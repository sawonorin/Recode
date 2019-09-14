using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Managers
{
    public interface IDepartmentService
    {
        Task<ExecutionResponse<DepartmentModelPage>> GetDepartments(string name = "", int pageSize = 10, int pageNo = 1);
        Task<ExecutionResponse<DepartmentModel>> GetDepartment(long Id);
        Task<ExecutionResponse<DepartmentModel>> CreateDepartment(DepartmentModel model);
        Task<ExecutionResponse<DepartmentModel>> UpdateDepartment(DepartmentModel model);
        Task<ExecutionResponse<object>> DeleteDepartment (long Id);
    }
}
