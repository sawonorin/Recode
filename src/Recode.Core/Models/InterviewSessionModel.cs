using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace Recode.Core.Models
{
    public class InterviewSessionModelPage
    {
        public InterviewSessionModel[] InterviewSessions { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
    }

    public class InterviewSessionModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long JobRoleId { get; set; }
        public string Subject { get; set; }
        public string JobRoleName { get; set; }
        public long RecruiterId { get; set; }
        public string RecruiterEmail { get; set; }
        public string Status { get; set; }
        public long? VenueId { get; set; }
        public string VenueName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class InterviewSessionVariable
    {
        public long InterviewSessionId { get; set; }
        public long[] Ids { get; set; }
    }
}
