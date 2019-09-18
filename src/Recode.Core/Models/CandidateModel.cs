using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace Recode.Core.Models
{
    public class CandidateModelPage
    {
        public CandidateModel[] Candidates { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
    }

    public class CandidateModel : UpdateCandidateModel
    {
        public long CompanyId { get; set; }
        public string JobRoleName { get; set; }
        public string DepartmentName { get; set; }
        public int InterviewStage { get; set; }
        public string Status { get; set; }
        public string ResumeUrl { get; set; }
    }

    public class UpdateCandidateModel
    {
        public long Id { get; set; }
        public long JobRoleId { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
    }
}
