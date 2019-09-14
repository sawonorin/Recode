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
    public class JobRoleService : IJobRoleService
    {
        private readonly IHttpContextExtensionService _httpContext;
        private readonly ISSOService _ssoService;
        private readonly IRepositoryCommand<JobRole, long> _jobRoleCommandRepo;
        private readonly IRepositoryQuery<JobRole, long> _jobRoleQueryRepo;
        private readonly IRepositoryQuery<Department, long> _departmentQueryRepo;
        private readonly IMapper _mapper;
        private long _currentCompanyId;
        public long CurrentCompanyId
        {
            get
            {
                _currentCompanyId = _currentCompanyId == 0 ? _httpContext.GetCurrentCompanyId() : _currentCompanyId;
                return _currentCompanyId;
            }
        }
        public JobRoleService(
            IRepositoryCommand<JobRole, long> jobRoleCommandRepo,
            IRepositoryQuery<JobRole, long> jobRoleQueryRepo,
            IRepositoryQuery<Department, long> departmentQueryRepo,
            IMapper mapper, IHttpContextExtensionService httpContext)
        {
            _jobRoleCommandRepo = jobRoleCommandRepo;
            _jobRoleQueryRepo = jobRoleQueryRepo;
            _departmentQueryRepo = departmentQueryRepo;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public async Task<ExecutionResponse<JobRoleModel>> CreateJobRole(JobRoleModel model)
        {
            var oldJobRole = _jobRoleQueryRepo.GetAll().FirstOrDefault(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower() && x.Department.CompanyId == CurrentCompanyId);

            if (oldJobRole != null)
                throw new Exception("Job Role already exists");
                       
            if (!_departmentQueryRepo.GetAll().Any(d => d.Id == model.DepartmentId && d.CompanyId == CurrentCompanyId))
                return new ExecutionResponse<JobRoleModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "Department does not exist"
                };

            //save jobRole info
            var jobRole = new JobRole
            {
                Name = model.Name,
                DepartmentId = model.DepartmentId,
                Description = model.Description,
                CreateById = _httpContext.GetCurrentUserId()
            };

            await _jobRoleCommandRepo.InsertAsync(jobRole);

            await _jobRoleCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<JobRoleModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<JobRoleModel>(jobRole)
            };
        }

        public async Task<ExecutionResponse<JobRoleModel>> GetJobRole(long Id)
        {
            var jobRole = _jobRoleQueryRepo.GetAll().Include(x=>x.Department).FirstOrDefault(x => x.Id == Id && x.Department.CompanyId == CurrentCompanyId);

            if (jobRole == null)
                return new ExecutionResponse<JobRoleModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            return new ExecutionResponse<JobRoleModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<JobRoleModel>(jobRole)
            };
        }

        public async Task<ExecutionResponse<JobRoleModelPage>> GetJobRoles(string name = "", long DepartmentId = 0, int pageSize = 10, int pageNo = 1)
        {
            var jobRoles = _jobRoleQueryRepo.GetAll().Include(x => x.Department).Where(x=>x.Department.CompanyId == CurrentCompanyId);

            jobRoles = string.IsNullOrEmpty(name) ? jobRoles : jobRoles.Where(x => x.Name.Contains(name));
            jobRoles = DepartmentId == 0? jobRoles : jobRoles.Where(x => x.DepartmentId == DepartmentId);

            jobRoles.OrderBy(x => x.DepartmentId).ThenBy(x => x.Name);

            jobRoles.Skip(pageSize * (pageNo - 1)).Take(pageSize);

            return new ExecutionResponse<JobRoleModelPage>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = new JobRoleModelPage
                {
                    PageSize = pageSize,
                    PageNo = pageNo,
                    JobRoles = _mapper.Map<JobRoleModel[]>(jobRoles.ToList())
                }
            };
        }

        public async Task<ExecutionResponse<JobRoleModel>> UpdateJobRole(UpdateJobRoleModel model)
        {
            var jobRole = _jobRoleQueryRepo.GetAll().FirstOrDefault(x => x.Id == model.Id && x.Department.CompanyId == CurrentCompanyId);

            if (jobRole == null)
                return new ExecutionResponse<JobRoleModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            if(!_departmentQueryRepo.GetAll().Any(d=>d.Id == model.DepartmentId && d.CompanyId == CurrentCompanyId))
                return new ExecutionResponse<JobRoleModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "Department does not exist"
                };

            //update jobRole record in db
            jobRole.Name = model.Name;
            jobRole.Description = model.Description;
            jobRole.DepartmentId = model.DepartmentId;

            await _jobRoleCommandRepo.UpdateAsync(jobRole);
            await _jobRoleCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<JobRoleModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<JobRoleModel>(jobRole)
            };
        }

        public async Task<ExecutionResponse<object>> DeleteJobRole(long Id)
        {
            var jobRole = _jobRoleQueryRepo.GetAll().FirstOrDefault(x => x.Id == Id && x.Department.CompanyId == CurrentCompanyId);

            if (jobRole == null)
                return new ExecutionResponse<object>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            try
            {
                await _jobRoleCommandRepo.DeleteAsync(jobRole);
                await _jobRoleCommandRepo.SaveChangesAsync();

                return new ExecutionResponse<object>
                {
                    ResponseCode = ResponseCode.Ok,
                    ResponseData = true
                };
            }
            catch (Exception ex)
            {
                return new ExecutionResponse<object>
                {
                    ResponseCode = ResponseCode.ServerException,
                    ResponseData = false
                };
            }
        }
    }
}
