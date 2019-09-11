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
        public long InterviewSessionMetricId { get; set; }
        public InterviewSessionMetric InterviewSessionMetric { get; set; }
        public int Rating { get; set; }
        [MaxLength(400)]
        public string Remark { get; set; }
    }
}
