using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace Recode.Core.Models
{
    //public class JobRoleModel
    //{
    //    public long Id { get; set; }
    //    public string Name { get; set; }
    //    public string Code { get; set; }
    //}

    public class JobRoleModelPage
    {
        public JobRoleModel[] JobRoles { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
    }

    public class UpdateJobRoleModel
    {
        public long Id { get; set; }
        public long DepartmentId { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
    }

    public class JobRoleModel : UpdateJobRoleModel
    {
        public string DepartmentName { get; set; }
    }
}
