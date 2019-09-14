using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Recode.Data.EntityBase;

namespace Recode.Data.AppEntity
{
    public class JobRole : Entity<long>
    {
        public long DepartmentId { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }        
        public Department Department { get; set; }
        public ICollection<Candidate> Candidates { get; set; }
    }
}
