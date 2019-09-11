using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Recode.Data.EntityBase;

namespace Recode.Data.AppEntity
{
    public class Metric : Entity<long>
    {
        public long? DepartmentId { get; set; }
        public long CompanyId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
    }
}
