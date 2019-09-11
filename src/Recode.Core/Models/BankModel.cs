using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Models
{
    public class BankModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string BankCode { get; set; }
        public string SortCode { get; set; }
    }
}
