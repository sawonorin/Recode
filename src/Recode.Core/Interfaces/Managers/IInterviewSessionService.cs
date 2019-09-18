using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Enums;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Managers
{
    public interface IInterviewSessionService
    {
        Task<ExecutionResponse<InterviewSessionModelPage>> GetInterviewSessions(string subject = "", long jobRoleId = 0, long recruiterId = 0, long venueId = 0, string status = "", int pageSize = 10, int pageNo = 1);
        Task<ExecutionResponse<InterviewSessionModel>> GetInterviewSession(long Id);
        //Task<ExecutionResponse<object>> DeleteInterviewSession(long Id);
        Task<ExecutionResponse<InterviewSessionModel>> CreateInterviewSession(InterviewSessionModel model);
        Task<ExecutionResponse<InterviewSessionModel>> UpdateInterviewSession(InterviewSessionModel model);
        Task<ExecutionResponse<InterviewSessionModel>> UpdateInterviewSessionStatus(long Id, InterviewSessionStatus status);
        Task<ExecutionResponse<object>> SetInterviewCandidates(long[] candidateIds, long interviewSessionId);
        Task<ExecutionResponse<object>> SetInterviewMetrics(long[] metricIds, long interviewSessionId);
        Task<ExecutionResponse<object>> SetInterviewInterviewers(long[] interviewerIds, long interviewSessionId);
    }
}
