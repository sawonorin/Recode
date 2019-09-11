using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Recode.Data.EntityBase;

namespace Recode.Data.AppEntity
{
    public class Role : Entity<long>
    {
        [Required]
        [MaxLength(10)]
        public string RoleName { get; set; }
        [Required]
        [MaxLength(10)]
        public string RoleType { get; set; }
        public string Description { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
