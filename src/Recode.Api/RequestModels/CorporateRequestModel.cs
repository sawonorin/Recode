using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recode.Api.RequestModels
{
    public class CorporateRequestModel : Model
    {
        public string BusinessAddress { get; set; }
        public string BusinessEmail { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string RCNumber { get; set; }
        public string PrimaryContactPhoneNumber { get; set; }
        public string AlternateContactPhoneNumber { get; set; }

    }
}
