using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Service.Models
{
    public class LoginResponseModel
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
        [JsonProperty(PropertyName = "expires_in")]
        public long ExpiryIn { get; set; }
    }
}
