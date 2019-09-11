using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Recode.Data.EntityBase;

namespace Recode.Data.AppEntity
{
    public class InterviewSessionResult : Entity<long>
    {
        public long CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        public long InterviewSessionId { get; set; }
        public InterviewSession InterviewSession { get; set; }
        public long MetricId { get; set; }
        public Metric Metric { get; set; }
        public int Rating { get; set; }
        [MaxLength(400)]
        public string Remark { get; set; }
    }
}
