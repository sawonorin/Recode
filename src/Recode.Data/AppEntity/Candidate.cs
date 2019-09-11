using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Recode.Data.EntityBase;
using Recode.Data.Enums;

namespace Recode.Data.AppEntity
{
    public class Candidate : Entity<long>
    {
        public Candidate()
        {
            Status = CandidateStatus.Ongoing;
        }

        public long CompanyId { get; set; }
        public long JobRoleId { get; set; }
        public JobRole JobRole { get; set; }
        public long DepartmentId { get; set; }
        public Department Department { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        public int InterviewStage { get; set; }
        public CandidateStatus Status { get; set; }
    }
}
