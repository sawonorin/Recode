using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Managers
{
    public interface ICandidateService
    {
        Task<ExecutionResponse<CandidateModelPage>> GetCandidates(string firstName = "", string lastName = "", string email = "", long jobRole = 0, int pageSize = 10, int pageNo = 1);
        Task<ExecutionResponse<CandidateModel>> GetCandidate(long Id);
        Task<ExecutionResponse<object>> DeleteCandidate(long Id);
        Task<ExecutionResponse<CandidateModel>> CreateCandidate(Stream fileStream, string contentType, UpdateCandidateModel model);
        Task<ExecutionResponse<CandidateModel>> UpdateCandidate(Stream fileStream, string contentType, UpdateCandidateModel model);
    }
}
