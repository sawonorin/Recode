using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.ConfigModels
{
    public class SSoSetting
    {
        public string SSOIdentityUrl { get; set; }
        public string SSOAPI { get; set; }
        public string EbipsUserId { get; set; }
        public string EbipsUserSecret { get; set; }
        public string EbipsClientId { get; set; }
        public string EbipsClientSecret { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string OrgUrl { get; set; }
        public string AppCode { get; set; }
    }
}
