using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Models
{
    public class CorporateModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string CorporateRcNumber { get; set; }


        public string BusinessAddress { get; set; }
        public string BusinessEmail { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PrimaryContactPhoneNumber { get; set; }
        public string AlternateContactPhoneNumber { get; set; }
    }
}
