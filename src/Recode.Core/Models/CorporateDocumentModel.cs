using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Models
{
    public class CorporateDocumentModel
    {
        public long CorporateId { get; set; }
        public long DocumentTypeId { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentType { get; set; }
    }
}
