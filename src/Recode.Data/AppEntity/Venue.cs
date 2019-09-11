using System;
using System.Collections.Generic;
using System.Text;
using Recode.Data.EntityBase;

namespace Recode.Data.AppEntity
{
    public class Venue : Entity<long>
    {
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
