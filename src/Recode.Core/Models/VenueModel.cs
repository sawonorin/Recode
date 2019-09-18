using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace Recode.Core.Models
{
    //public class VenueModel
    //{
    //    public long Id { get; set; }
    //    public string Name { get; set; }
    //    public string Code { get; set; }
    //}

    public class VenueModelPage
    {
        public VenueModel[] Venues { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
    }

    public class VenueModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
