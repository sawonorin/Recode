using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Models
{
    public class RoleModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public enum RoleType
    {
        vgg_superadmin,
        vgg_admin,
        vgg_user,
        clientadmin,
        clientuser
    }
}
