using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Recode.Core.ConfigModels;
using Recode.Core.Interfaces.Managers;
using Recode.Core.Interfaces.Services;
using Recode.Core.Models;
using Recode.Data.AppEntity;
using Recode.Repository.CoreRepositories;
using Recode.Service.SSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Recode.Service.EntityService
{
    public class UserService : IUserService
    {
        private readonly IHttpContextExtensionService _httpContext;
        private readonly ISSOService _ssoService;
        private readonly IRepositoryCommand<User, long> _userCommandRepo;
        private readonly IRepositoryQuery<User, long> _userQueryRepo;
        private readonly IRepositoryCommand<UserRole, long> _userRoleCommandRepo;
        private readonly IRepositoryQuery<Role, long> _roleQueryRepo;
        private readonly IRepositoryQuery<Company, long> _companyQueryRepo;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IRepositoryQuery<UserRole, long> _userRoleQueryRepo;
        private readonly SSoSetting _ssoSettings;

        private readonly string CurrentUserId;
        private long _currentCompanyId;
        public long CurrentCompanyId
        {
            get
            {
                _currentCompanyId = _currentCompanyId == 0 ? _httpContext.GetCurrentCompanyId() : _currentCompanyId;
                return _currentCompanyId;
            }
        }

        public UserService(ISSOService ssoService,
            IRepositoryCommand<User, long> userCommandRepo,
            IRepositoryQuery<User, long> userQueryRepo,
            IRepositoryCommand<UserRole, long> userRoleCommandRepo,
            IRepositoryQuery<UserRole, long> userRoleQueryRepo,
            IRepositoryQuery<Role, long> roleQueryRepo,
            IRepositoryQuery<Company, long> companyQueryRepo,
            IEmailService emailService,
            IMapper mapper,
             IOptions<SSoSetting> sSoSetting, IHttpContextExtensionService httpContext)
        {
            _ssoSettings = sSoSetting.Value;
            _ssoService = ssoService;
            _userCommandRepo = userCommandRepo;
            _userQueryRepo = userQueryRepo;
            _userRoleCommandRepo = userRoleCommandRepo;
            _roleQueryRepo = roleQueryRepo;
            _companyQueryRepo = companyQueryRepo;
            _emailService = emailService;
            _mapper = mapper;
            _userRoleQueryRepo = userRoleQueryRepo;
            _httpContext = httpContext;

            CurrentUserId = _httpContext.GetCurrentSSOUserId();
        }

        public async Task<ExecutionResponse<UserModel>> AddUserRole(UserRoleModel model)
        {
            var role = _roleQueryRepo.GetAll().FirstOrDefault(f => f.Id == model.RoleId);
            if (role == null)
            {
                return new ExecutionResponse<UserModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "Role does not exist"
                };
            }
            
            if (CurrentCompanyId == 0 && role.RoleName != "CompanyAdmin")
                throw new Exception($"You cannot add role '{role.RoleName}' to a user");

            var user = _userQueryRepo.GetAll().FirstOrDefault(f => f.Id == model.UserId);
            if (user == null || (CurrentCompanyId != 0 && user.CompanyId != CurrentCompanyId))
            {
                return new ExecutionResponse<UserModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "User does not exist"
                };
            }

            if (!user.UserRoles.Any(u => u.RoleId == role.Id))
            {
                var result = await _ssoService.AddRemoveClaims(new UserClaimModel { Claims = new List<SSOClaim> { new SSOClaim("role", role.RoleName) }, UserId = user.SSOUserId }, ClaimAction.Add);

                if (result.ResponseCode == ResponseCode.Ok)
                {
                    await _userRoleCommandRepo.InsertAsync(new UserRole { RoleId = role.Id, UserId = user.Id });
                    await _userRoleCommandRepo.SaveChangesAsync();

                    return await GetUser(model.UserId);
                }
                else
                {
                    return new ExecutionResponse<UserModel>
                    {
                        ResponseCode = ResponseCode.ServerException,
                        Message = result.Message
                    };
                }
            }
            else
            {
                return new ExecutionResponse<UserModel>
                {
                    ResponseCode = ResponseCode.ServerException,
                    Message = "User is already attached to specified role"
                };
            }
        }

        public async Task<ExecutionResponse<UserModel>> CreateUser(CreateUserModel model)
        {
            if (_userQueryRepo.GetAll().Any(u => u.Email.Trim().ToLower() == model.Email.Trim().ToLower()))
                return new ExecutionResponse<UserModel>
                {
                    ResponseCode = ResponseCode.ServerException,
                    Message = "User with specified email already exists"
                };

            var role = _roleQueryRepo.GetAll().FirstOrDefault(x => x.Id == model.RoleId );
            if (role == null)
                throw new Exception("Invalid Role");

            if (CurrentCompanyId == 0 && role.RoleName != "CompanyAdmin")
                throw new Exception($"You cannot create a user with role '{role.RoleName}'");

            var company = _companyQueryRepo.GetAll().FirstOrDefault(x => x.Id == model.CompanyId);
            if (company == null)
                throw new Exception("Company does not exist");

            var registerReponse = await _ssoService.Register(new SSOUserDto
            {
                Claims = new List<SSOClaim> { new SSOClaim("role", GetSSORole(role.RoleName)),
                                            new SSOClaim("role", role.RoleName) },
                ConfirmEmail = false,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? "08111111111" : model.PhoneNumber,
                UserName = model.UserName,
                Password = model.Password
            });

            if (registerReponse.ResponseCode != ResponseCode.Ok)
                return new ExecutionResponse<UserModel>
                {
                    ResponseCode = ResponseCode.ServerException,
                    Message = registerReponse.Message
                };

            //user registered on sso 
            //trigger reset password

            //save user info
            var user = new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? "08111111111" : model.PhoneNumber,
                UserName = model.UserName,
                CompanyId = CurrentCompanyId == 0 ? model.CompanyId : CurrentCompanyId,
                EmailConfirmed = false,
                CreateById = CurrentUserId,
                SSOUserId = registerReponse.ResponseData.UserId
            };
            await _userCommandRepo.InsertAsync(user);
            await _userCommandRepo.SaveChangesAsync();

            //assign role to user
            await _userRoleCommandRepo.InsertAsync(new UserRole { RoleId = role.Id, UserId = user.Id, CreateById = CurrentUserId });
            await _userRoleCommandRepo.SaveChangesAsync();

            //send email confirmation mail
            _emailService.EmailConfirmation(registerReponse.ResponseData.EmailConfirmationToken, model.Email, $"{model.FirstName} {model.LastName}", registerReponse.ResponseData.UserId);


            return new ExecutionResponse<UserModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<UserModel>(user)
            };
        }

        private string GetSSORole(string roleName)
        {
            switch (roleName)
            {
                case "VGG_Admin":
                    return "vgg_admin";
                case "CompanyAdmin":
                    return "clientadmin";
                default:
                    return "clientuser";
            }
        }

        public async Task<ExecutionResponse<UserModel>> GetUser(string ssoUserId)
        {
            var user = _userQueryRepo.GetAll().FirstOrDefault(x => x.SSOUserId == ssoUserId);

            if (user == null)
                return new ExecutionResponse<UserModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            return new ExecutionResponse<UserModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<UserModel>(user)
            };
        }

        public async Task<ExecutionResponse<UserModel>> GetUser(long Id)
        {
            var user = _userQueryRepo.GetAll().FirstOrDefault(x => x.Id == Id);

            if (user == null || (CurrentCompanyId != 0 && user.CompanyId != CurrentCompanyId))
                return new ExecutionResponse<UserModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            return new ExecutionResponse<UserModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<UserModel>(user)
            };
        }

        public async Task<ExecutionResponse<UserModel>> GetUserByEmail(string email)
        {
            var user = _userQueryRepo.GetAll().FirstOrDefault(x => x.Email.Trim().ToLower() == email.Trim().ToLower());

            if (user == null || (CurrentCompanyId != 0 && user.CompanyId != CurrentCompanyId))
                return new ExecutionResponse<UserModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            return new ExecutionResponse<UserModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<UserModel>(user)
            };
        }

        public async Task<ExecutionResponse<UserModel[]>> GetUsersByRoleId(long roleId)
        {
            var userRoles = _userRoleQueryRepo.GetAll().Include(x => x.User).Where(x => x.RoleId == roleId)
                            .Select(x => _mapper.Map<UserModel>(x.User)).ToArray();

            userRoles = CurrentCompanyId == 0 ? userRoles : userRoles.Where(u => u.CompanyId == CurrentCompanyId).ToArray();

            return new ExecutionResponse<UserModel[]>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = userRoles
            };
        }

        public async Task<ExecutionResponse<UserModel[]>> GetUsers(long[] userIds)
        {
            var users = _userQueryRepo.GetAll().Where(x => userIds.Contains(x.Id)).ToArray();

            users = CurrentCompanyId == 0 ? users : users.Where(u => u.CompanyId == CurrentCompanyId).ToArray();

            return new ExecutionResponse<UserModel[]>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<UserModel[]>(users)
            };
        }

        public async Task<ExecutionResponse<UserModelPage>> GetUsers(string email = "", string firstName = "", string lastName = "", string userName = "", long roleId = 0, int pageSize = 10, int pageNo = 1)
        {

            var users = roleId == 0 ? _userQueryRepo.GetAll().Select(x=> _mapper.Map<UserModel>(x)) :_userRoleQueryRepo.GetAll().Include(x => x.User).Where(x => x.RoleId == roleId)
                            .Select(x => _mapper.Map<UserModel>(x.User));
           
            users = CurrentCompanyId == 0 ? users : users.Where(x => x.CompanyId == CurrentCompanyId);
            users = string.IsNullOrEmpty(email) ? users : users.Where(x => x.Email.Contains(email));
            users = string.IsNullOrEmpty(firstName) ? users : users.Where(x => x.FirstName.Contains(firstName));
            users = string.IsNullOrEmpty(lastName) ? users : users.Where(x => x.LastName.Contains(lastName));
            users = string.IsNullOrEmpty(userName) ? users : users.Where(x => x.UserName.Contains(userName));

            users.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ThenBy(x => x.Email).ThenBy(x => x.UserName);

            users.Skip(pageSize * (pageNo - 1)).Take(pageSize);

            return new ExecutionResponse<UserModelPage>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = new UserModelPage
                {
                    PageSize = pageSize,
                    PageNo = pageNo,
                    Users = users.ToArray()
                }
            };
        }

        public async Task<ExecutionResponse<UserModel>> RemoveUserRole(UserRoleModel model)
        {
            var userRole = _userRoleQueryRepo.GetAll().FirstOrDefault(x => x.UserId == model.UserId && x.RoleId == model.RoleId);
            if (userRole != null)
            {
                var role = _roleQueryRepo.GetAll().FirstOrDefault(f => f.Id == model.RoleId);
                if (role == null)
                {
                    return new ExecutionResponse<UserModel>
                    {
                        ResponseCode = ResponseCode.NotFound,
                        Message = "Role does not exist"
                    };
                }

                var user = _userQueryRepo.GetAll().FirstOrDefault(f => f.Id == model.UserId);
                if (user == null || (CurrentCompanyId != 0 && user.CompanyId != CurrentCompanyId))
                {
                    return new ExecutionResponse<UserModel>
                    {
                        ResponseCode = ResponseCode.NotFound,
                        Message = "User does not exist"
                    };
                }

                var result = await _ssoService.AddRemoveClaims(new UserClaimModel { Claims = new List<SSOClaim> { new SSOClaim("role", role.RoleName) }, UserId = user.SSOUserId }, ClaimAction.Remove);

                if (result.ResponseCode == ResponseCode.Ok)
                {
                    await _userRoleCommandRepo.DeleteAsync(userRole.Id);
                    await _userRoleCommandRepo.SaveChangesAsync();

                    return await GetUser(model.UserId);
                }
                else
                {
                    return new ExecutionResponse<UserModel>
                    {
                        ResponseCode = ResponseCode.ServerException,
                        Message = result.Message
                    };
                }
            }
            else
            {
                return new ExecutionResponse<UserModel>
                {
                    ResponseCode = ResponseCode.ServerException,
                    Message = "User is not attached to specified role"
                };
            }
        }

        public async Task<ExecutionResponse<UserModel>> ToggleActivateUser(long Id)
        {
            var user = _userQueryRepo.GetAllIncludeInactive().FirstOrDefault(x => x.Id == Id);

            if (user == null || (CurrentCompanyId != 0 && user.CompanyId != CurrentCompanyId))
                return new ExecutionResponse<UserModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            user.IsActive = !user.IsActive;

            await _userCommandRepo.UpdateAsync(user);
            await _userCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<UserModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<UserModel>(user)
            };
        }

        public async Task<ExecutionResponse<UserModel>> UpdateUser(UserModel model)
        {
            var user = _userQueryRepo.GetAll().FirstOrDefault(x => x.Id == model.Id);

            if (user == null || (CurrentCompanyId != 0 && user.CompanyId != CurrentCompanyId))
                return new ExecutionResponse<UserModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            var ssoUpdate = await _ssoService.UpdateUser(new SSOUpdateUserDto
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName
            });

            if (ssoUpdate.ResponseCode != ResponseCode.Ok)
                return new ExecutionResponse<UserModel>
                {
                    ResponseCode = ResponseCode.ServerException,
                    Message = ssoUpdate.Message
                };

            //update user record in db
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.LastName = model.LastName;
            user.FirstName = model.FirstName;

            await _userCommandRepo.UpdateAsync(user);
            await _userCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<UserModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<UserModel>(user)
            };
        }

        public async Task<bool> ResendEmailConfirmation(long UserId)
        {
            var user = _userQueryRepo.GetAll().FirstOrDefault(x => x.Id == UserId);

            if (user == null)
                throw new Exception("No record found");

            var ssoresponse = await _ssoService.GetConfirmationToken(user.SSOUserId);

            if (ssoresponse.ResponseCode != ResponseCode.Ok)
                return false;

            return _emailService.EmailConfirmation(ssoresponse.ResponseData.EmailConfirmationToken, user.Email, $"{user.FirstName} {user.LastName}", user.SSOUserId);
        }

        public async Task<ExecutionResponse<UserModel>> OnboardAdmin(OnboardUserModel model)
        {
            var ssoUser = await _ssoService.GetUser(model.Id);

            if (ssoUser.ResponseCode != ResponseCode.Ok)
            {
                return new ExecutionResponse<UserModel>
                {

                };
            }

            var user = new User
            {
                SSOUserId = model.Id,
                FirstName = ssoUser.ResponseData.Response.FirstName,
                LastName = ssoUser.ResponseData.Response.LastName,
                UserName = ssoUser.ResponseData.Response.UserName,
                Email = ssoUser.ResponseData.Response.Email,
                CreateById = "VGGSuperAdmin",
                PhoneNumber = ssoUser.ResponseData.Response.PhoneNumber,
                EmailConfirmed = ssoUser.ResponseData.EmailConfirmed,
                CompanyId = 0
            };

            await _userCommandRepo.InsertAsync(user);
            await _userCommandRepo.SaveChangesAsync();

            var role = _roleQueryRepo.GetAll().FirstOrDefault(r => r.RoleName == "VGG_Admin");

            if (role != null)
            {
                await _userRoleCommandRepo.InsertAsync(new UserRole { RoleId = role.Id, UserId = user.Id, CreateById = "VGGSuperAdmin" });
                await _userRoleCommandRepo.SaveChangesAsync();
            }

            return new ExecutionResponse<UserModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<UserModel>(user)
            };
        }
    }
}
