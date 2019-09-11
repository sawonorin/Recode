using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Recode.Data.EntityBase;

namespace Recode.Data.AppEntity
{
    public class InterviewSessionInterviewer : Entity<long>
    {
        public long InterviewerId { get; set; }
        [ForeignKey("InterviewerId")]
        public User Interviewer{ get; set; }
        public long InterviewSessionId { get; set; }
       // public InterviewSession InterviewSession { get; set; }
    }
}
