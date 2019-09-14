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
    public class CompanyService : ICompanyService
    {
        private readonly IHttpContextExtensionService _httpContext;
        private readonly ISSOService _ssoService;
        private readonly IRepositoryCommand<Company, long> _companyCommandRepo;
        private readonly IRepositoryQuery<Company, long> _companyQueryRepo;
        private readonly IMapper _mapper;

        private readonly string CurrentCompanyId;
        public CompanyService(
            IRepositoryCommand<Company, long> companyCommandRepo,
            IRepositoryQuery<Company, long> companyQueryRepo,
            IMapper mapper, IHttpContextExtensionService httpContext)
        {
            _companyCommandRepo = companyCommandRepo;
            _companyQueryRepo = companyQueryRepo;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public async Task<ExecutionResponse<CompanyModel>> CreateCompany(CreateCompanyModel model)
        {
            var oldCompany = _companyQueryRepo.GetAll().FirstOrDefault(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower());

            if (oldCompany != null)
                throw new Exception("Company already exists");

            //save company info
            var company = new Company
            {
                Name = model.Name,
                Code = model.Code,
                CreateById = _httpContext.GetCurrentUserId()
            };

            await _companyCommandRepo.InsertAsync(company);

            await _companyCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<CompanyModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<CompanyModel>(company)
            };
        }

        public async Task<ExecutionResponse<CompanyModel>> GetCompany(long Id)
        {
            var company = _companyQueryRepo.GetAll().FirstOrDefault(x => x.Id == Id);

            if (company == null)
                return new ExecutionResponse<CompanyModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            return new ExecutionResponse<CompanyModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<CompanyModel>(company)
            };
        }

        public async Task<ExecutionResponse<CompanyModelPage>> GetCompanys(string name = "", string code = "", int pageSize = 10, int pageNo = 1)
        {
            var companys = _companyQueryRepo.GetAll();

            companys = string.IsNullOrEmpty(name) ? companys : companys.Where(x => x.Name.Contains(name));
            companys = string.IsNullOrEmpty(code) ? companys : companys.Where(x => x.Code.Contains(code));

            companys.OrderBy(x => x.Name).ThenBy(x => x.Code);

            companys.Skip(pageSize * (pageNo - 1)).Take(pageSize);

            return new ExecutionResponse<CompanyModelPage>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = new CompanyModelPage
                {
                    PageSize = pageSize,
                    PageNo = pageNo,
                    Companys = _mapper.Map<CompanyModel[]>(companys.ToList())
                }
            };
        }

        public async Task<ExecutionResponse<CompanyModel>> UpdateCompany(CompanyModel model)
        {
            var company = _companyQueryRepo.GetAll().FirstOrDefault(x => x.Id == model.Id);

            if (company == null)
                return new ExecutionResponse<CompanyModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };
            
            //update company record in db
            company.Name = model.Name;
            company.Code = model.Code;

            await _companyCommandRepo.UpdateAsync(company);
            await _companyCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<CompanyModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<CompanyModel>(company)
            };
        }
    }
}
