using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.ConfigModels
{
    public class MailSetting
    {
        public string MailFrom { get; set; }
        public string MailFromName { get; set; }
        public string SMTPServer { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassword { get; set; }
        public string SMTPPORT { get; set; }
    }
}
