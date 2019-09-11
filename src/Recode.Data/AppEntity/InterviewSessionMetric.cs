using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Recode.Data.EntityBase;

namespace Recode.Data.AppEntity
{
    public class InterviewSessionMetric : Entity<long>
    {
        public long MetricId { get; set; }
        public Metric Metric { get; set; }
        public long InterviewSessionId { get; set; }
        public InterviewSession InterviewSession { get; set; }
    }
}
