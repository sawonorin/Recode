using System;
using System.Collections.Generic;
using System.Text;
using Recode.Data.EntityBase;

namespace Recode.Data.AppEntity
{
    public class Company : Entity<long>
    {
        public string Name { get; set;}
        public string Code { get; set; }
    }
}
