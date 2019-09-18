using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Managers
{
    public interface IMetricService
    {
        Task<ExecutionResponse<MetricModelPage>> GetMetrics(string name = "", long departmentId = 0, int pageSize = 10, int pageNo = 1);
        Task<ExecutionResponse<MetricModel>> GetMetric(long Id);
        Task<ExecutionResponse<object>> DeleteMetric(long Id);
        Task<ExecutionResponse<MetricModel>> CreateMetric(UpdateMetricModel model);
        Task<ExecutionResponse<MetricModel>> UpdateMetric(UpdateMetricModel model);
    }
}
