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
    public class DepartmentService : IDepartmentService
    {
        private readonly IHttpContextExtensionService _httpContext;
        private readonly ISSOService _ssoService;
        private readonly IRepositoryCommand<Department, long> _departmentCommandRepo;
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

        public DepartmentService(
            IRepositoryCommand<Department, long> departmentCommandRepo,
            IRepositoryQuery<Department, long> departmentQueryRepo,
            IMapper mapper, IHttpContextExtensionService httpContext)
        {
            _departmentCommandRepo = departmentCommandRepo;
            _departmentQueryRepo = departmentQueryRepo;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public async Task<ExecutionResponse<DepartmentModel>> CreateDepartment(DepartmentModel model)
        {
            var oldDepartment = _departmentQueryRepo.GetAll().FirstOrDefault(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower());

            if (oldDepartment != null)
                throw new Exception("Department already exists");

            //save department info
            var department = new Department
            {
                Name = model.Name,
                CompanyId = CurrentCompanyId,
                Description = model.Description,
                CreateById = _httpContext.GetCurrentSSOUserId()
            };

            await _departmentCommandRepo.InsertAsync(department);

            await _departmentCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<DepartmentModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<DepartmentModel>(department)
            };
        }

        public async Task<ExecutionResponse<DepartmentModel>> GetDepartment(long Id)
        {
            var department = _departmentQueryRepo.GetAll().FirstOrDefault(x => x.Id == Id && x.CompanyId == CurrentCompanyId);

            if (department == null)
                return new ExecutionResponse<DepartmentModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            return new ExecutionResponse<DepartmentModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<DepartmentModel>(department)
            };
        }

        public async Task<ExecutionResponse<DepartmentModelPage>> GetDepartments(string name = "", int pageSize = 10, int pageNo = 1)
        {
            var departments = _departmentQueryRepo.GetAll().Where(x => x.CompanyId == CurrentCompanyId);

            departments = string.IsNullOrEmpty(name) ? departments : departments.Where(x => x.Name.Contains(name));

            departments.OrderBy(x => x.Name);

            departments.Skip(pageSize * (pageNo - 1)).Take(pageSize);

            return new ExecutionResponse<DepartmentModelPage>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = new DepartmentModelPage
                {
                    PageSize = pageSize,
                    PageNo = pageNo,
                    Departments = _mapper.Map<DepartmentModel[]>(departments.ToList())
                }
            };
        }

        public async Task<ExecutionResponse<DepartmentModel>> UpdateDepartment(DepartmentModel model)
        {
            var department = _departmentQueryRepo.GetAll().FirstOrDefault(x => x.Id == model.Id && x.CompanyId == CurrentCompanyId);

            if (department == null)
                return new ExecutionResponse<DepartmentModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };
                       
            //update department record in db
            department.Name = model.Name;
            department.Description = model.Description;

            await _departmentCommandRepo.UpdateAsync(department);
            await _departmentCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<DepartmentModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<DepartmentModel>(department)
            };
        }

        public async Task<ExecutionResponse<object>> DeleteDepartment(long Id)
        {
            var department = _departmentQueryRepo.GetAll().FirstOrDefault(x => x.Id == Id && x.CompanyId == CurrentCompanyId);

            if (department == null)
                return new ExecutionResponse<object>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            try
            {
                await _departmentCommandRepo.DeleteAsync(department);
                await _departmentCommandRepo.SaveChangesAsync();

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
