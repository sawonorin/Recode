using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace Recode.Core.Models
{
    //public class MetricModel
    //{
    //    public long Id { get; set; }
    //    public string Name { get; set; }
    //    public string Code { get; set; }
    //}

    public class MetricModelPage
    {
        public MetricModel[] Metrics { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
    }

    public class UpdateMetricModel
    {
        public long Id { get; set; }
        public long? DepartmentId { get; set; }
        public long CompanyId { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
    }

    public class MetricModel : UpdateMetricModel
    {
        public string DepartmentName { get; set; }
    }
}
