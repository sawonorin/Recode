using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Recode.Data.EntityBase;

namespace Recode.Data.AppEntity
{
    public class UserRole : Entity<long>
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
    }
}
