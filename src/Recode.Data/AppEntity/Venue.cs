using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Recode.Data.EntityBase;

namespace Recode.Data.AppEntity
{
    public class Venue : Entity<long>
    {
        public long CompanyId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
