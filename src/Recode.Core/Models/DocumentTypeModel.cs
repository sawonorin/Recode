using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Models
{
    public class DocumentTypeModel
    {
        public long Id { get; set; }
        public string DocumentName { get; set; }
        public string DocumentCode { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
    }
}
