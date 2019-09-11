using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Models
{
    public class PermissionModel
    {
        public long Id { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
