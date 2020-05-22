using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Recode.Data.EntityBase;
using Recode.Core.Enums;

namespace Recode.Data.AppEntity
{
    public class Candidate : Entity<long>
    {
        public Candidate()
        {
            Status = CandidateStatus.Ongoing.ToString();
            InterviewStage = 0;
        }

        public long CompanyId { get; set; }
        public long JobRoleId { get; set; }
        public JobRole JobRole { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        public int InterviewStage { get; set; }
        public string Status { get; set; }
        public string ResumeUrl { get; set; }
    }
}
