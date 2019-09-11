using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Managers
{
    public interface ICompanyService
    {
        Task<ExecutionResponse<CompanyModelPage>> GetCompanys(string name = "", string code = "", int pageSize = 10, int pageNo = 1);
        Task<ExecutionResponse<CompanyModel>> GetCompany(long Id);
        Task<ExecutionResponse<CompanyModel>> CreateCompany(CreateCompanyModel model);
        Task<ExecutionResponse<CompanyModel>> UpdateCompany(CompanyModel model);
    }
}
