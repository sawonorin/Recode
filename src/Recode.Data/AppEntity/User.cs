using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Recode.Data.EntityBase;

namespace Recode.Data.AppEntity
{
    public class User: Entity<long>
    {
        [Required]
        public string SSOUserId { get; set; }
        public long CompanyId { get; set; }
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }
        [MaxLength(50)]
        [Required]
        public string Email { get; set; }
        [MaxLength(50)]
        [Required]
        public string UserName { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
