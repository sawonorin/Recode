using System;
using IdentityModel.Client;
using System.Text;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using IdentityModel;
using System.Linq;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Recode.Service.SSO;
using Recode.Core.ConfigModels;
using Recode.Core.Models;
using Recode.Service;
using Recode.Service.Utilities;

namespace Recode.Service.SSO
{
    public class SSOService : ISSOService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SSoSetting _ssoConstants;

        public SSOService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor, IOptions<SSoSetting> ssoConstants)
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
            _ssoConstants = ssoConstants.Value;
        }

        private async Task<HttpClient> GetClient(bool isCurrentClient = false)
        {
            var clienttoken = await GetClientToken();

            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(_ssoConstants.SSOAPI);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", clienttoken);

            client.DefaultRequestHeaders.Add("client-id", isCurrentClient ? _ssoConstants.ClientId  : _ssoConstants.EbipsClientId);

            return client;
        }

        public async Task<ExecutionResponse<LoginResDto>> Login(LoginDto loginDto)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                client.BaseAddress = new Uri(_ssoConstants.SSOIdentityUrl);
                var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = "/identity/connect/token",
                    ClientId = _ssoConstants.ClientId,
                    ClientSecret = _ssoConstants.ClientSecret,
                    Scope = "openid profile email roles identity-server-api",
                    UserName = loginDto.UserName,
                    Password = loginDto.Password
                });
                var responseObj = JsonConvert.DeserializeObject<LoginResDto>(response.Raw);
                if (!response.IsError)
                {
                    return new ExecutionResponse<LoginResDto>
                    {
                        Message = "Login token retrieved successful",
                        ResponseCode = ResponseCode.Ok,
                        ResponseData = responseObj
                    };
                }
                else
                {
                    return new ExecutionResponse<LoginResDto>
                    {
                        Message = $"{response.Error} - {response.ErrorDescription}",
                        ResponseCode = ResponseCode.ValidationError
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<LoginResDto>
                {
                    Message = "An Error occurred while ,processing your request!",
                    ResponseCode = ResponseCode.ValidationError
                };
            }
        }

        public async Task<ExecutionResponse<SSOEmailConfirmationTokenResponseDto>> Register(SSOUserDto registerDto)
        {
            try
            {
                registerDto.PhoneNumber = string.IsNullOrEmpty(registerDto.PhoneNumber) ? "08011111111" : registerDto.PhoneNumber;

                var claims = GenerateClaims(registerDto.Claims);

                //registerDto.Claims = new List<Claim>();

                var client = await GetClient();

                var dtoString = JsonConvert.SerializeObject(registerDto);

                StringContent httpContent = new StringContent(dtoString, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/account/register", httpContent);
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode || (int)response.StatusCode == 302)
                {
                    //302 => user already exists on sso

                    var resObj = response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<SSOEmailConfirmationTokenResponseDto>(result) : new SSOEmailConfirmationTokenResponseDto
                    {
                        UserId = JsonConvert.DeserializeObject<string>(result)
                    };

                    //extend user claim... add application to user claim
                    var addClaimResponse = await AddRemoveClaims(new UserClaimModel { UserId = resObj.UserId, Claims = claims }, ClaimAction.Add);

                    //add clearance for user
                    var addClearanceResponse = await AddClearance(new ClearanceModel { UserId = resObj.UserId, ClientIds = new List<string> { _ssoConstants.ClientId }, Enabled = true });

                    //eventually, we will remove this line
                    //addClearanceResponse = await AddClearance(new ClearanceModel { UserId = resObj.UserId, ClientIds = new List<string> { _ssoConstants.ClientUserId }, Enabled = true });

                    return new ExecutionResponse<SSOEmailConfirmationTokenResponseDto>
                    {
                        Message = "User registered on SSO successfully",
                        ResponseCode = ResponseCode.Ok,
                        ResponseData = resObj
                    };
                }
                else
                    return new ExecutionResponse<SSOEmailConfirmationTokenResponseDto>
                    {
                        Message = $"User was not registered on SSO successfully - {response.ReasonPhrase}",
                        ResponseCode = ResponseCode.ValidationError,
                        ResponseData = null
                    };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<SSOEmailConfirmationTokenResponseDto>
                {
                    Message = "User was not registered on SSO successfully - " + ex.Message + ex.InnerException + ex.Source,
                    ResponseCode = ResponseCode.ServerException,
                    ResponseData = null
                };
            }
        }

        private List<Claim> GenerateClaims(List<Claim> claims)
        {
            List<Claim> newClaims = new List<Claim>();

            newClaims.Add(new Claim("application", _ssoConstants.ClientId));

            foreach (var item in claims)
            {
                newClaims.Add(new Claim("role", item.Value));
            }

            return newClaims;
        }

        public async Task<ExecutionResponse<SSOEmailConfirmationTokenResponseDto>> GetConfirmationToken(string UserId)
        {
            try
            {
                var client = await GetClient();

                var queryendpointstring = ($"/account/generateconfirmtoken?userId={UserId}");
                var response = await client.PostAsync(queryendpointstring, null);
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var resObj = JsonConvert.DeserializeObject<SSOEmailConfirmationTokenResponseDto>(result);
                    return new ExecutionResponse<SSOEmailConfirmationTokenResponseDto>
                    {
                        Message = "User email confirmation Code retrieved on SSO successfully",
                        ResponseCode = ResponseCode.Ok,
                        ResponseData = resObj
                    };
                }
                else
                {
                    return new ExecutionResponse<SSOEmailConfirmationTokenResponseDto>
                    {
                        Message = $"An error occured - {result}",
                        ResponseCode = ResponseCode.ServerException,
                        ResponseData = null
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<SSOEmailConfirmationTokenResponseDto>
                {
                    Message = "User was not registered on SSO successfully" + ex.Message + ex.InnerException + ex.Source,
                    ResponseCode = ResponseCode.ServerException,
                    ResponseData = null
                };
            }
        }

        public async Task<ExecutionResponse<SSOUser>> GetUser(string userId)
        {
            try
            {
                var client = await GetClient();

                //client.DefaultRequestHeaders.Add("client-id", "web.ebips.user");
                var response = await client.GetAsync("/account/getuser?userId=" + userId);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var resObj = JsonConvert.DeserializeObject<SSOUser>(result);
                    return new ExecutionResponse<SSOUser>
                    {
                        Message = "User retrieved on SSO successfully",
                        ResponseCode = ResponseCode.Ok,
                        ResponseData = resObj
                    };
                }
                else
                {
                    return new ExecutionResponse<SSOUser>
                    {
                        Message = "An error occurred while retrieving User email confirmation Code from on SSO",
                        ResponseCode = ResponseCode.ServerException,
                        ResponseData = null
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<SSOUser>
                {
                    Message = "UserId not found " + ex.Message + ex.InnerException + ex.Source,
                    ResponseCode = ResponseCode.ServerException,
                    ResponseData = null
                };
            }
        }

        public async Task<ExecutionResponse<SSOUpdateUserResDto>> UpdateUser(SSOUpdateUserDto sSOUserDto)
        {
            try
            {
                var client = await GetClient();

                var dtoString = JsonConvert.SerializeObject(sSOUserDto);
                StringContent httpContent = new StringContent(dtoString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/account/update", httpContent);
                var result = await response.Content.ReadAsStringAsync();

                var resObj = JsonConvert.DeserializeObject<SSOUpdateUserResDto>(result);
                if (response.IsSuccessStatusCode)
                {
                    return new ExecutionResponse<SSOUpdateUserResDto>
                    {
                        Message = "User details updated successfully",
                        ResponseCode = ResponseCode.Ok,
                        ResponseData = resObj
                    };
                }
                else
                {
                    return new ExecutionResponse<SSOUpdateUserResDto>
                    {
                        Message = $"{result}",
                        ResponseCode = ResponseCode.ServerException,
                        ResponseData = null
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<SSOUpdateUserResDto>
                {
                    Message = "An error occurred while processing your request",
                    ResponseCode = ResponseCode.ServerException,
                    ResponseData = null
                };
            }
        }

        public async Task<ExecutionResponse<List<Claim>>> GetUserClaims(string ssoUserId)
        {
            try
            {
                var client = await GetClient(isCurrentClient: true);

                var response = await client.GetAsync($"/account/userclaims?userId={ssoUserId}");

                var result = await response.Content.ReadAsStringAsync();

                var resObj = JsonConvert.DeserializeObject<List<Claim>>(result);
                if (response.IsSuccessStatusCode)
                {
                    return new ExecutionResponse<List<Claim>>
                    {
                        Message = "",
                        ResponseCode = ResponseCode.Ok,
                        ResponseData = resObj
                    };
                }
                else
                {
                    return new ExecutionResponse<List<Claim>>
                    {
                        Message = $"{result}",
                        ResponseCode = ResponseCode.ServerException,
                        ResponseData = null
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<List<Claim>>
                {
                    Message = $"An error occurred while processing your request - {ex.Message}",
                    ResponseCode = ResponseCode.ServerException,
                    ResponseData = null
                };
            }
        }
        public async Task<ExecutionResponse<object>> AddRemoveClaims(UserClaimModel claims, ClaimAction action)
        {
            try
            {
                var client = await GetClient(isCurrentClient: true);

                var dtoString = JsonConvert.SerializeObject(claims);
                StringContent httpContent = new StringContent(dtoString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"/account/{(action == ClaimAction.Add ? "addclaims" : "removeclaims")}", httpContent);

                var result = await response.Content.ReadAsStringAsync();

                var resObj = JsonConvert.DeserializeObject<object>(result);
                if (response.IsSuccessStatusCode)
                {
                    return new ExecutionResponse<object>
                    {
                        Message = $"User claims {(action == ClaimAction.Add ? "added" : "removed")} successfully",
                        ResponseCode = ResponseCode.Ok,
                        ResponseData = resObj
                    };
                }
                else
                {
                    return new ExecutionResponse<object>
                    {
                        Message = $"{result}",
                        ResponseCode = ResponseCode.ServerException,
                        ResponseData = null
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<object>
                {
                    Message = "An error occurred while processing your request",
                    ResponseCode = ResponseCode.ServerException,
                    ResponseData = null
                };
            }
        }
        public async Task<ExecutionResponse<object>> AddClearance(ClearanceModel model)
        {
            try
            {
                var client = await GetClient();

                var dtoString = JsonConvert.SerializeObject(model);
                StringContent httpContent = new StringContent(dtoString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"/clearance/add", httpContent);

                var result = await response.Content.ReadAsStringAsync();

                var resObj = JsonConvert.DeserializeObject<object>(result);
                if (response.IsSuccessStatusCode)
                {
                    return new ExecutionResponse<object>
                    {
                        Message = $"successful",
                        ResponseCode = ResponseCode.Ok,
                        ResponseData = resObj
                    };
                }
                else
                {
                    return new ExecutionResponse<object>
                    {
                        Message = "An error occurred while processing your request",
                        ResponseCode = ResponseCode.ServerException,
                        ResponseData = null
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<object>
                {
                    Message = "An error occurred while processing your request",
                    ResponseCode = ResponseCode.ServerException,
                    ResponseData = null
                };
            }
        }

        public async Task<ExecutionResponse<SSOForgotPasswordResponse>> ForgotPassword(string email)
        {
            try
            {
                var client = await GetClient();
                var response = await client.GetAsync($"/account/forgotpassword?email={email}");
                var result = await response.Content.ReadAsStringAsync();

                var resObj = JsonConvert.DeserializeObject<SSOForgotPasswordResponse>(result);
                if (response.IsSuccessStatusCode)
                {
                    return new ExecutionResponse<SSOForgotPasswordResponse>
                    {
                        Message = $"",
                        ResponseCode = ResponseCode.Ok,
                        ResponseData = resObj
                    };
                }
                else
                {
                    return new ExecutionResponse<SSOForgotPasswordResponse>
                    {
                        Message = "An error occurred while processing your request",
                        ResponseCode = ResponseCode.ServerException,
                        ResponseData = null
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<SSOForgotPasswordResponse>
                {
                    Message = "An error occurred while processing your request",
                    ResponseCode = ResponseCode.ServerException,
                    ResponseData = null
                };
            }
        }

        public async Task<ExecutionResponse<object>> ChangePassword(SSOChangePasswordRequestModel model)
        {
            try
            {
                var client = await GetClient();

                var dtoString = JsonConvert.SerializeObject(model);
                StringContent httpContent = new StringContent(dtoString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"/account/changepassword", httpContent);

                var result = await response.Content.ReadAsStringAsync();

                var resObj = JsonConvert.DeserializeObject<object>(result);
                if (response.IsSuccessStatusCode)
                {
                    return new ExecutionResponse<object>
                    {
                        Message = $"",
                        ResponseCode = ResponseCode.Ok,
                        ResponseData = resObj
                    };
                }
                else
                {
                    return new ExecutionResponse<object>
                    {
                        Message = "An error occurred while processing your request",
                        ResponseCode = ResponseCode.ServerException,
                        ResponseData = null
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<object>
                {
                    Message = "An error occurred while processing your request",
                    ResponseCode = ResponseCode.ServerException,
                    ResponseData = null
                };
            }
        }

        public async Task<ExecutionResponse<object>> ResetPassword(SSOResetPasswordRequestModel model)
        {
            try
            {
                var client = await GetClient();

                var dtoString = JsonConvert.SerializeObject(model);
                StringContent httpContent = new StringContent(dtoString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"/account/resetpassword", httpContent);

                var result = await response.Content.ReadAsStringAsync();

                var resObj = JsonConvert.DeserializeObject<object>(result);
                if (response.IsSuccessStatusCode)
                {
                    return new ExecutionResponse<object>
                    {
                        Message = $"",
                        ResponseCode = ResponseCode.Ok,
                        ResponseData = resObj
                    };
                }
                else
                {
                    return new ExecutionResponse<object>
                    {
                        Message = "An error occurred while processing your request",
                        ResponseCode = ResponseCode.ServerException,
                        ResponseData = null
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<object>
                {
                    Message = "An error occurred while processing your request",
                    ResponseCode = ResponseCode.ServerException,
                    ResponseData = null
                };
            }
        }
        public async Task<ExecutionResponse<object>> ConfirmUser(SSOConfirmUserModel model)
        {
            try
            {
                var client = await GetClient();

                var dtoString = JsonConvert.SerializeObject(model);
                StringContent httpContent = new StringContent(dtoString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"/account/confirm", httpContent);

                var result = await response.Content.ReadAsStringAsync();

                var resObj = JsonConvert.DeserializeObject<object>(result);
                if (response.IsSuccessStatusCode)
                {
                    return new ExecutionResponse<object>
                    {
                        Message = $"",
                        ResponseCode = ResponseCode.Ok,
                        ResponseData = resObj
                    };
                }
                else
                {
                    return new ExecutionResponse<object>
                    {
                        Message = "An error occurred while processing your request",
                        ResponseCode = ResponseCode.ServerException,
                        ResponseData = null
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<object>
                {
                    Message = "An error occurred while processing your request",
                    ResponseCode = ResponseCode.ServerException,
                    ResponseData = null
                };
            }
        }

        //public string GetAuthClientToken()
        //{
        //    try
        //    {

        //        DateTime datetokenExpiry;
        //        try
        //        {
        //            datetokenExpiry = DateTime.Parse(tokenExpiry);
        //        }
        //        catch
        //        {
        //            datetokenExpiry = new DateTime(1970, 1, 1);
        //        }
        //        if (!string.IsNullOrEmpty(token))
        //        {
        //            if (DateTime.Now.ToLocalTime() >= datetokenExpiry)
        //            {
        //                return GetClientCredentials().Result;
        //            }
        //            else
        //            {
        //                return token;
        //            }
        //        }
        //        else
        //        {
        //            return GetClientCredentials().Result;
        //        }
        //    }
        //    catch
        //    {
        //        return GetClientCredentials().Result;
        //    }
        //}
        private async Task<string> GetClientToken()
        {
            try
            {
                //_httpContextAccessor.HttpContext.Response.Cookies.Delete("clientId-token");

                var token = _httpContextAccessor.HttpContext.Request.Cookies["clientId-token"];
                if (!string.IsNullOrEmpty(token))
                    return token;

                //token already expired or cannot be found

                var client = _clientFactory.CreateClient();
                client.BaseAddress = new Uri(_ssoConstants.SSOIdentityUrl);

                var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = "/identity/connect/token",
                    ClientId = _ssoConstants.EbipsClientId,
                    ClientSecret = _ssoConstants.EbipsClientSecret, //Constants.EbipsClientSecret,
                    Scope = $"identity-server-api ebipsgatewayapi"
                });
                if (!string.IsNullOrEmpty(response.AccessToken))
                {
                    _httpContextAccessor.HttpContext.Response.Cookies.Append($"clientId-token", response.AccessToken, new CookieOptions { Expires = DateTimeOffset.Now.AddMinutes(45) });
                }
                return response.AccessToken;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return string.Empty;
            }
        }
    }
}
