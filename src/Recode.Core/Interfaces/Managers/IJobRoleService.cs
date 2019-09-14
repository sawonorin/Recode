using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Managers
{
    public interface IJobRoleService
    {
        Task<ExecutionResponse<JobRoleModelPage>> GetJobRoles(string name = "", long departmentId = 0, int pageSize = 10, int pageNo = 1);
        Task<ExecutionResponse<JobRoleModel>> GetJobRole(long Id);
        Task<ExecutionResponse<object>> DeleteJobRole(long Id);
        Task<ExecutionResponse<JobRoleModel>> CreateJobRole(JobRoleModel model);
        Task<ExecutionResponse<JobRoleModel>> UpdateJobRole(UpdateJobRoleModel model);
    }
}
