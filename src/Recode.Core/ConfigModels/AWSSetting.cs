using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.ConfigModels
{
    public class AWSSettings
    {
        public string BucketName { get; set; }
        public string ServiceUrl { get; set; }
        public string ApplicationFolder { get; set; }
    }
}
