﻿using System;
namespace Recode.Service
{
    public class LoginResDto
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
    }
}
