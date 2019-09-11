using System;
namespace Recode.Service
{
    public class SSOUpdateUserResDto
    {
        public string EmailConfirmed { get; set; }
        public string Exists { get; set; }
        public string Response { get; set; }
        public string CustomValue { get; set; }
        public string TotalRecords { get; set; }
    }
}
