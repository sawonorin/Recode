using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Recode.Data.EntityBase;
using Recode.Core.Enums;

namespace Recode.Data.AppEntity
{
    public class InterviewSession : Entity<long>
    {
        public long CompanyId { get; set; }
        public long JobRoleId { get; set; }
        public string Subject { get; set; }
        public JobRole JobRole { get; set; }
        public long RecruiterId { get; set; }
        [ForeignKey("RecruiterId")]
        public User Recruiter { get; set; }
        public string Status { get; set; }
        public long? VenueId { get; set; }
        public Venue Venue { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
