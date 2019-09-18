using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Recode.Core.ConfigModels;
using Recode.Core.Interfaces.Managers;
using Recode.Core.Interfaces.Services;
using Recode.Core.Models;
using Recode.Data.AppEntity;
using Recode.Repository.CoreRepositories;
using Recode.Service.SSO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VigiPay.Org.Service.EntityService;

namespace Recode.Service.EntityService
{
    public class CandidateService : ICandidateService
    {
        private readonly IHttpContextExtensionService _httpContext;
        private readonly IRepositoryCommand<Candidate, long> _candidateCommandRepo;
        private readonly IRepositoryQuery<Candidate, long> _candidateQueryRepo;
        private readonly IRepositoryQuery<JobRole, long> _jobRoleQueryRepo;
        private readonly IS3Service _s3Service;
        private readonly IMapper _mapper;
        private long _currentCompanyId;
        public long CurrentCompanyId
        {
            get
            {
                CurrentCompanyId1 = CurrentCompanyId1 == 0 ? _httpContext.GetCurrentCompanyId() : CurrentCompanyId1;
                return CurrentCompanyId1;
            }
        }

        public long CurrentCompanyId1 { get => _currentCompanyId; set => _currentCompanyId = value; }

        public CandidateService(
            IRepositoryCommand<Candidate, long> candidateCommandRepo,
            IRepositoryQuery<Candidate, long> candidateQueryRepo,
            IRepositoryQuery<JobRole, long> jobRoleQueryRepo,
            IS3Service s3Service, IMapper mapper, IHttpContextExtensionService httpContext)
        {
            _candidateCommandRepo = candidateCommandRepo;
            _candidateQueryRepo = candidateQueryRepo;
            _jobRoleQueryRepo = jobRoleQueryRepo;
            _s3Service = s3Service;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public async Task<ExecutionResponse<CandidateModel>> CreateCandidate(Stream fileStream, string contentType, UpdateCandidateModel model)
        {
            var oldCandidate = _candidateQueryRepo.GetAll().FirstOrDefault(x => x.Email.Trim().ToLower() == model.Email.Trim().ToLower());

            if (oldCandidate != null)
                throw new Exception("Candidate already exists");

            var jobRole = _jobRoleQueryRepo.GetAll().FirstOrDefault(j => j.Id == model.JobRoleId);
            if (jobRole == null)
                throw new Exception("Job Role does not exist");

            //upload resume to s3 bucket
            var s3result = await _s3Service.UploadFile(fileStream, $"{model.Email}", contentType);

            if (s3result.ResponseCode == ResponseCode.Ok)
                throw new Exception("Could not create candidate - resume upload failed");

            //save candidate info
            var candidate = new Candidate
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                JobRoleId = model.JobRoleId,
                PhoneNumber = model.PhoneNumber,
                ResumeUrl = s3result.ResponseData,
                CompanyId = CurrentCompanyId,
                CreateById = _httpContext.GetCurrentSSOUserId()
            };

            await _candidateCommandRepo.InsertAsync(candidate);
            await _candidateCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<CandidateModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<CandidateModel>(candidate)
            };
        }

        public async Task<ExecutionResponse<CandidateModel>> GetCandidate(long Id)
        {
            var candidate = _candidateQueryRepo.GetAll().FirstOrDefault(x => x.Id == Id && x.CompanyId == CurrentCompanyId);

            if (candidate == null)
                return new ExecutionResponse<CandidateModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            return new ExecutionResponse<CandidateModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<CandidateModel>(candidate)
            };
        }

        public async Task<ExecutionResponse<CandidateModelPage>> GetCandidates(string firstName = "", string lastName = "", string email = "", long jobRoleId = 0, int pageSize = 10, int pageNo = 1)
        {
            var candidates = _candidateQueryRepo.GetAll().Where(x => x.CompanyId == CurrentCompanyId);

            candidates = string.IsNullOrEmpty(firstName) ? candidates : candidates.Where(x => x.FirstName.Contains(firstName));
            candidates = string.IsNullOrEmpty(lastName) ? candidates : candidates.Where(x => x.LastName.Contains(lastName));
            candidates = string.IsNullOrEmpty(email) ? candidates : candidates.Where(x => x.Email.Contains(email));
            candidates = jobRoleId == 0 ? candidates : candidates.Where(x => x.JobRoleId == jobRoleId);
            candidates.OrderBy(x => x.JobRoleId).ThenBy(x => x.FirstName).ThenBy(x=>x.LastName).ThenBy(x=>x.Email);

            candidates.Skip(pageSize * (pageNo - 1)).Take(pageSize);

            return new ExecutionResponse<CandidateModelPage>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = new CandidateModelPage
                {
                    PageSize = pageSize,
                    PageNo = pageNo,
                    Candidates = _mapper.Map<CandidateModel[]>(candidates.ToList())
                }
            };
        }

        public async Task<ExecutionResponse<CandidateModel>> UpdateCandidate(Stream fileStream, string contentType, UpdateCandidateModel model)
        {
            var candidate = _candidateQueryRepo.GetAll().FirstOrDefault(x => x.Id == model.Id && x.CompanyId == CurrentCompanyId);

            if (candidate == null)
                return new ExecutionResponse<CandidateModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            var jobRole = _jobRoleQueryRepo.GetAll().FirstOrDefault(j => j.Id == model.JobRoleId);
            if (jobRole == null)
                throw new Exception("Job Role does not exist");

            if (fileStream != null)
            {
                var s3result = await _s3Service.UploadFile(fileStream, model.Email, contentType);
                if (s3result.ResponseCode == ResponseCode.Ok)
                    throw new Exception("Could not update candidate - resume upload failed");
            }

            //update candidate record in db
            candidate.FirstName = model.FirstName;
            candidate.LastName = model.LastName;
            candidate.PhoneNumber = model.PhoneNumber;
            candidate.Email = model.Email;
            candidate.JobRoleId = model.JobRoleId;

            await _candidateCommandRepo.UpdateAsync(candidate);
            await _candidateCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<CandidateModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<CandidateModel>(candidate)
            };
        }

        public async Task<ExecutionResponse<object>> DeleteCandidate(long Id)
        {
            var candidate = _candidateQueryRepo.GetAll().FirstOrDefault(x => x.Id == Id && x.CompanyId == CurrentCompanyId);

            if (candidate == null)
                return new ExecutionResponse<object>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            try
            {
                await _candidateCommandRepo.DeleteAsync(candidate);
                await _candidateCommandRepo.SaveChangesAsync();

                return new ExecutionResponse<object>
                {
                    ResponseCode = ResponseCode.Ok,
                    ResponseData = true
                };
            }
            catch (Exception ex)
            {
                return new ExecutionResponse<object>
                {
                    ResponseCode = ResponseCode.ServerException,
                    ResponseData = false
                };
            }
        }
    }
}