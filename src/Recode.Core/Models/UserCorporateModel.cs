using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Models
{
    public class UserCorporateModel
    {
        public string UserId { get; set; }
        public long CorporateId { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}
