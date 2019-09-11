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
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IRepositoryQuery<UserRole, long> _userRoleQueryRepo;
        private readonly SSoSetting _ssoSettings;

        private readonly string CurrentUserId;
        public UserService(ISSOService ssoService,
            IRepositoryCommand<User, long> userCommandRepo,
            IRepositoryQuery<User, long> userQueryRepo,
            IRepositoryCommand<UserRole, long> userRoleCommandRepo,
            IRepositoryQuery<UserRole, long> userRoleQueryRepo,
            IRepositoryQuery<Role, long> roleQueryRepo,
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
            _emailService = emailService;
            _mapper = mapper;
            _userRoleQueryRepo = userRoleQueryRepo;
            _httpContext = httpContext;

            CurrentUserId = _httpContext.GetCurrentUserId();
        }

        public async Task<ExecutionResponse<UserModel>> AddUserRole(UserRoleModel model)
        {
            await _userRoleCommandRepo.InsertAsync(new UserRole { CreateById = CurrentUserId, RoleId = model.RoleId, UserId = model.UserId });
            await _userRoleCommandRepo.SaveChangesAsync();
            return await GetUser(model.UserId);
        }

        public async Task<ExecutionResponse<UserModel>> CreateUser(CreateUserModel model)
        {
            var role = _roleQueryRepo.GetAll().FirstOrDefault(x => x.Id == model.RoleId);

            if (role == null)
                throw new Exception("Invalid Role");

            var registerReponse = await _ssoService.Register(new SSOUserDto
            {
                Claims = new List<Claim> { new Claim("role", GetSSORole(role.RoleName)),
                                            new Claim("role", role.RoleName) },
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
            //save user info
            var user = new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? "08111111111" : model.PhoneNumber,
                UserName = model.UserName,
                CompanyId = model.CompanyId,
                EmailConfirmed = false,
                CreateById = CurrentUserId,
                SSOUserId = registerReponse.ResponseData.UserId
            };
            await _userCommandRepo.InsertAsync(user);

            await _userCommandRepo.SaveChangesAsync();

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

        public async Task<ExecutionResponse<UserModel>> GetUserByEmail(string email)
        {
            var user = _userQueryRepo.GetAll().FirstOrDefault(x => x.Email.Trim().ToLower() == email.Trim().ToLower());

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

        public async Task<ExecutionResponse<UserModel[]>> GetUsersByRoleId(long roleId)
        {
            var userRoles = _userRoleQueryRepo.GetAll().Include(x=>x.User).Where(x => x.RoleId == roleId)
                            .Select(x => new[] { x.User }).ToList();

            return new ExecutionResponse<UserModel[]>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<UserModel[]>(userRoles)
            };
        }        

        public async Task<ExecutionResponse<UserModel[]>> GetUsers(long[] userIds)
        {
            var users = _userQueryRepo.GetAll().Where(x => userIds.Contains(x.Id));

            return new ExecutionResponse<UserModel[]>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<UserModel[]>(users)
            };
        }

        public async Task<ExecutionResponse<UserModelPage>> GetUsers(string email = "", string firstName = "", string lastName = "", string userName = "", int pageSize = 10, int pageNo = 1)
        {
            var users = _userQueryRepo.GetAll().Where(x => x.CompanyId == _httpContext.GetCurrentCompanyId());

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
                    Users = _mapper.Map<UserModel[]>(users.ToList())
                }
            };
        }

        public async Task<ExecutionResponse<UserModel>> RemoveUserRole(UserRoleModel model)
        {
            var userRole = _userRoleQueryRepo.GetAll().FirstOrDefault(x => x.UserId == model.UserId && x.RoleId == model.RoleId);
            if (userRole != null)
                await _userRoleCommandRepo.DeleteAsync(userRole.Id);
            await _userRoleCommandRepo.SaveChangesAsync();

            return await GetUser(model.UserId);
        }

        public async Task<ExecutionResponse<UserModel>> ToggleActivateUser(long Id)
        {
             var user = _userQueryRepo.GetAll().FirstOrDefault(x => x.Id == Id);

            if (user == null)
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

            if (user == null)
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
    }
}
