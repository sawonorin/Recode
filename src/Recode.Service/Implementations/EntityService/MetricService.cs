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
    public class MetricService : IMetricService
    {
        private readonly IHttpContextExtensionService _httpContext;
        private readonly IRepositoryCommand<Metric, long> _metricCommandRepo;
        private readonly IRepositoryQuery<Metric, long> _metricQueryRepo;
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
        public MetricService(
            IRepositoryCommand<Metric, long> metricCommandRepo,
            IRepositoryQuery<Metric, long> metricQueryRepo,
            IRepositoryQuery<Department, long> departmentQueryRepo,
            IMapper mapper, IHttpContextExtensionService httpContext)
        {
            _metricCommandRepo = metricCommandRepo;
            _metricQueryRepo = metricQueryRepo;
            _departmentQueryRepo = departmentQueryRepo;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public async Task<ExecutionResponse<MetricModel>> CreateMetric(UpdateMetricModel model)
        {
            var oldMetric = _metricQueryRepo.GetAll().FirstOrDefault(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower() && x.CompanyId == CurrentCompanyId);

            if (oldMetric != null)
                throw new Exception("Metric already exists");
        
            //save metric info
            var metric = new Metric
            {
                Name = model.Name,
                DepartmentId = model.DepartmentId,
                Description = model.Description,
                CompanyId = CurrentCompanyId,
                CreateById = _httpContext.GetCurrentSSOUserId()
            };

            await _metricCommandRepo.InsertAsync(metric);
            await _metricCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<MetricModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<MetricModel>(metric)
            };
        }

        public async Task<ExecutionResponse<MetricModel>> GetMetric(long Id)
        {
            var metric = _metricQueryRepo.GetAll().Include(x=>x.Department).FirstOrDefault(x => x.Id == Id && x.CompanyId == CurrentCompanyId);

            if (metric == null)
                return new ExecutionResponse<MetricModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            return new ExecutionResponse<MetricModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<MetricModel>(metric)
            };
        }

        public async Task<ExecutionResponse<MetricModelPage>> GetMetrics(string name = "", long DepartmentId = 0, int pageSize = 10, int pageNo = 1)
        {
            var metrics = _metricQueryRepo.GetAll().Include(x => x.Department).Where(x=>x.CompanyId == CurrentCompanyId);

            metrics = string.IsNullOrEmpty(name) ? metrics : metrics.Where(x => x.Name.Contains(name));
            metrics = DepartmentId == 0? metrics : metrics.Where(x => x.DepartmentId == DepartmentId);

            metrics.OrderBy(x => x.DepartmentId).ThenBy(x => x.Name);

            metrics.Skip(pageSize * (pageNo - 1)).Take(pageSize);

            return new ExecutionResponse<MetricModelPage>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = new MetricModelPage
                {
                    PageSize = pageSize,
                    PageNo = pageNo,
                    Metrics = _mapper.Map<MetricModel[]>(metrics.ToList())
                }
            };
        }

        public async Task<ExecutionResponse<MetricModel>> UpdateMetric(UpdateMetricModel model)
        {
            var metric = _metricQueryRepo.GetAll().FirstOrDefault(x => x.Id == model.Id && x.CompanyId == CurrentCompanyId);

            if (metric == null)
                return new ExecutionResponse<MetricModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            if(!_departmentQueryRepo.GetAll().Any(d=>d.Id == model.DepartmentId && d.CompanyId == CurrentCompanyId))
                return new ExecutionResponse<MetricModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "Department does not exist"
                };

            //update metric record in db
            metric.Name = model.Name;
            metric.Description = model.Description;
            metric.DepartmentId = model.DepartmentId;

            await _metricCommandRepo.UpdateAsync(metric);
            await _metricCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<MetricModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<MetricModel>(metric)
            };
        }

        public async Task<ExecutionResponse<object>> DeleteMetric(long Id)
        {
            var metric = _metricQueryRepo.GetAll().FirstOrDefault(x => x.Id == Id && x.CompanyId == CurrentCompanyId);

            if (metric == null)
                return new ExecutionResponse<object>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            try
            {
                await _metricCommandRepo.DeleteAsync(metric);
                await _metricCommandRepo.SaveChangesAsync();

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
