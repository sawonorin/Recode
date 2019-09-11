using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace Recode.Core.Models
{
    public class CompanyModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class CompanyModelPage
    {
        public CompanyModel[] Companys { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
    }

    public class CreateCompanyModel
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [MaxLength(15)]
        public string Code { get; set; }
    }
}
