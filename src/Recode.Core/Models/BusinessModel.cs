using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Models
{
    public class BusinessModel
    {
        public long Id { get; set; }
        public long CorporateId { get; set; }
        public string BusinessCode { get; set; }
        public string Description { get; set; }
        public string BusinessName { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
