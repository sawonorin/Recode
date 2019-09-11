using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Recode.Data.EntityBase;

namespace Recode.Data.AppEntity
{
    public class InterviewSessionCandidate : Entity<long>
    {
        public long CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        public long InterviewSessionId { get; set; }
        public InterviewSession InterviewSession { get; set; }
    }
}
