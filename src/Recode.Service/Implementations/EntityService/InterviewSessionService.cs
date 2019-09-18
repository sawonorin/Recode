using AutoMapper;
using Recode.Core.Enums;
using Recode.Core.Interfaces.Managers;
using Recode.Core.Interfaces.Services;
using Recode.Core.Models;
using Recode.Data.AppEntity;
using Recode.Repository.CoreRepositories;
using Recode.Service.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Recode.Service.EntityService
{
    public class InterviewSessionService : IInterviewSessionService
    {
        private readonly IHttpContextExtensionService _httpContext;
        private readonly IRepositoryQuery<InterviewSession, long> _interviewSessionQueryRepo;
        private readonly IRepositoryCommand<InterviewSession, long> _interviewSessionCommandRepo;
        private readonly IRepositoryQuery<InterviewSessionMetric, long> _interviewSessionMetricQueryRepo;
        private readonly IRepositoryCommand<InterviewSessionMetric, long> _interviewSessionMetricCommandRepo;
        private readonly IRepositoryQuery<InterviewSessionInterviewer, long> _interviewSessionInterviewerQueryRepo;
        private readonly IRepositoryCommand<InterviewSessionInterviewer, long> _interviewSessionInterviewerCommandRepo;
        private readonly IRepositoryQuery<InterviewSessionCandidate, long> _interviewSessionCandidateQueryRepo;
        private readonly IRepositoryCommand<InterviewSessionCandidate, long> _interviewSessionCandidateCommandRepo;
        private readonly IMapper _mapper;
        private long _currentCompanyId;
        public long CurrentCompanyId
        {
            get
            {
                _currentCompanyId = _currentCompanyId == 0 ? _httpContext.GetCurrentCompanyId() : _currentCompanyId;
                return _currentCompanyId;
            }
        }

        public InterviewSessionService(
            IRepositoryCommand<InterviewSession, long> interviewSessionCommandRepo,
            IRepositoryQuery<InterviewSession, long> interviewSessionQueryRepo,
            IRepositoryQuery<InterviewSessionMetric, long> interviewSessionMetricQueryRepo,
            IRepositoryCommand<InterviewSessionMetric, long> interviewSessionMetricCommandRepo,
            IRepositoryQuery<InterviewSessionInterviewer, long> interviewSessionInterviewerQueryRepo,
            IRepositoryCommand<InterviewSessionInterviewer, long> interviewSessionInterviewerCommandRepo,
            IRepositoryQuery<InterviewSessionCandidate, long> interviewSessionCandidateQueryRepo,
            IRepositoryCommand<InterviewSessionCandidate, long> interviewSessionCandidateCommandRepo,
            IMapper mapper, IHttpContextExtensionService httpContext)
        {
            _interviewSessionCommandRepo = interviewSessionCommandRepo;
            _interviewSessionQueryRepo = interviewSessionQueryRepo;
            _interviewSessionMetricQueryRepo = interviewSessionMetricQueryRepo;
            _interviewSessionMetricCommandRepo = interviewSessionMetricCommandRepo;
            _interviewSessionInterviewerQueryRepo = interviewSessionInterviewerQueryRepo;
            _interviewSessionInterviewerCommandRepo = interviewSessionInterviewerCommandRepo;
            _interviewSessionCandidateQueryRepo = interviewSessionCandidateQueryRepo;
            _interviewSessionCandidateCommandRepo = interviewSessionCandidateCommandRepo;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public async Task<ExecutionResponse<InterviewSessionModel>> CreateInterviewSession(InterviewSessionModel model)
        {
            //save interviewSession info
            var interviewSession = new InterviewSession
            {
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Subject = model.Subject,
                VenueId = model.VenueId,
                RecruiterId = _httpContext.GetCurrentUserId(),
                CompanyId = CurrentCompanyId,
                JobRoleId = model.JobRoleId,
                CreateById = _httpContext.GetCurrentSSOUserId()
            };

            await _interviewSessionCommandRepo.InsertAsync(interviewSession);
            await _interviewSessionCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<InterviewSessionModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<InterviewSessionModel>(interviewSession)
            };
        }

        public async Task<ExecutionResponse<InterviewSessionModel>> GetInterviewSession(long Id)
        {
            var interviewSession = _interviewSessionQueryRepo.GetAll().FirstOrDefault(x => x.Id == Id && x.CompanyId == CurrentCompanyId);

            if (interviewSession == null)
                return new ExecutionResponse<InterviewSessionModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            return new ExecutionResponse<InterviewSessionModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<InterviewSessionModel>(interviewSession)
            };
        }

        public async Task<ExecutionResponse<InterviewSessionModelPage>> GetInterviewSessions(string subject = "", long jobRoleId = 0, long recruiterId = 0, long venueId = 0, string status = "", int pageSize = 10, int pageNo = 1)
        {
            var interviewSessions = _interviewSessionQueryRepo.GetAll().Where(x => x.CompanyId == CurrentCompanyId);

            interviewSessions = string.IsNullOrEmpty(subject) ? interviewSessions : interviewSessions.Where(x => x.Subject.Contains(subject));
            interviewSessions = string.IsNullOrEmpty(status) ? interviewSessions : interviewSessions.Where(x => x.Status.Contains(status));
            interviewSessions = jobRoleId == 0 ? interviewSessions : interviewSessions.Where(x => x.JobRoleId == jobRoleId);
            interviewSessions = venueId == 0 ? interviewSessions : interviewSessions.Where(x => x.VenueId == venueId);
            interviewSessions = jobRoleId == 0 ? interviewSessions : interviewSessions.Where(x => x.RecruiterId == recruiterId);

            interviewSessions.OrderByDescending(x => x.StartTime);

            interviewSessions.Skip(pageSize * (pageNo - 1)).Take(pageSize);

            return new ExecutionResponse<InterviewSessionModelPage>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = new InterviewSessionModelPage
                {
                    PageSize = pageSize,
                    PageNo = pageNo,
                    InterviewSessions = _mapper.Map<InterviewSessionModel[]>(interviewSessions.ToList())
                }
            };
        }

        public async Task<ExecutionResponse<InterviewSessionModel>> UpdateInterviewSession(InterviewSessionModel model)
        {
            var interviewSession = _interviewSessionQueryRepo.GetAll().FirstOrDefault(x => x.Id == model.Id && x.CompanyId == CurrentCompanyId);

            if (interviewSession == null)
                return new ExecutionResponse<InterviewSessionModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            //update interviewSession record in db
            interviewSession.Subject = model.Subject;
            interviewSession.EndTime = model.EndTime;
            interviewSession.StartTime = model.StartTime;
            interviewSession.VenueId = model.VenueId;
            interviewSession.JobRoleId = model.JobRoleId;

            await _interviewSessionCommandRepo.UpdateAsync(interviewSession);
            await _interviewSessionCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<InterviewSessionModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<InterviewSessionModel>(interviewSession)
            };
        }
         public async Task<ExecutionResponse<InterviewSessionModel>> UpdateInterviewSessionStatus(long Id, InterviewSessionStatus status)
        {
            var interviewSession = _interviewSessionQueryRepo.GetAll().FirstOrDefault(x => x.Id == Id && x.CompanyId == CurrentCompanyId);

            if (interviewSession == null)
                return new ExecutionResponse<InterviewSessionModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            //update interviewSession record in db
            interviewSession.Status = status.ToString();

            await _interviewSessionCommandRepo.UpdateAsync(interviewSession);
            await _interviewSessionCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<InterviewSessionModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<InterviewSessionModel>(interviewSession)
            };
        }

        public async Task<ExecutionResponse<object>> SetInterviewCandidates(long[] candidateIds, long interviewSessionId)
        {
            try
            {
                var interviewSession = _interviewSessionQueryRepo.GetAll().FirstOrDefault(x => x.Id == interviewSessionId && x.CompanyId == CurrentCompanyId);

                if (interviewSession == null)
                    throw new Exception("Interview session does not exist");

                var interviewCandidates = _interviewSessionCandidateQueryRepo.GetAll().Where(x => x.InterviewSessionId == interviewSessionId);

                //to add
                foreach (var id in candidateIds)
                {
                    if (!interviewCandidates.Select(x => x.Id).ToArray().Contains(id))
                    {
                        await _interviewSessionCandidateCommandRepo.InsertAsync(new InterviewSessionCandidate { InterviewSessionId = interviewSessionId, CandidateId = id, CreateById = _httpContext.GetCurrentSSOUserId() });
                    }
                }

                //to remove
                foreach (var item in interviewCandidates.ToList())
                {
                    if (!candidateIds.Contains(item.Id))
                        await _interviewSessionCandidateCommandRepo.DeleteAsync(item);
                }

                await _interviewSessionCandidateCommandRepo.SaveChangesAsync();

                return new ExecutionResponse<object>
                {
                    ResponseData = true,
                    ResponseCode = ResponseCode.Ok
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<object>
                {
                    ResponseData = false,
                    ResponseCode = ResponseCode.ServerException,
                    Message = ex.Message
                };
            }
        }

        public async Task<ExecutionResponse<object>> SetInterviewMetrics(long[] metricIds, long interviewSessionId)
        {
            try
            {
                var interviewSession = _interviewSessionQueryRepo.GetAll().FirstOrDefault(x => x.Id == interviewSessionId && x.CompanyId == CurrentCompanyId);

                if (interviewSession == null)
                    throw new Exception("Interview session does not exist");

                var interviewMetrics = _interviewSessionMetricQueryRepo.GetAll().Where(x => x.InterviewSessionId == interviewSessionId);

                //to add
                foreach (var id in metricIds)
                {
                    if (!interviewMetrics.Select(x => x.Id).ToArray().Contains(id))
                    {
                        await _interviewSessionMetricCommandRepo.InsertAsync(new InterviewSessionMetric { InterviewSessionId = interviewSessionId, MetricId = id, CreateById = _httpContext.GetCurrentSSOUserId() });
                    }
                }

                //to remove
                foreach (var item in interviewMetrics.ToList())
                {
                    if (!metricIds.Contains(item.Id))
                        await _interviewSessionMetricCommandRepo.DeleteAsync(item);
                }

                await _interviewSessionMetricCommandRepo.SaveChangesAsync();

                return new ExecutionResponse<object>
                {
                    ResponseData = true,
                    ResponseCode = ResponseCode.Ok
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<object>
                {
                    ResponseData = false,
                    ResponseCode = ResponseCode.ServerException,
                    Message = ex.Message
                };
            }
        }

        public async Task<ExecutionResponse<object>> SetInterviewInterviewers(long[] interviewerIds, long interviewSessionId)
        {
            try
            {
                var interviewSession = _interviewSessionQueryRepo.GetAll().FirstOrDefault(x => x.Id == interviewSessionId && x.CompanyId == CurrentCompanyId);

                if (interviewSession == null)
                    throw new Exception("Interview session does not exist");

                var interviewInterviewers = _interviewSessionInterviewerQueryRepo.GetAll().Where(x => x.InterviewSessionId == interviewSessionId);

                //to add
                foreach (var id in interviewerIds)
                {
                    if (!interviewInterviewers.Select(x => x.Id).ToArray().Contains(id))
                    {
                        await _interviewSessionInterviewerCommandRepo.InsertAsync(new InterviewSessionInterviewer { InterviewSessionId = interviewSessionId, InterviewerId = id, CreateById = _httpContext.GetCurrentSSOUserId() });
                    }
                }

                //to remove
                foreach (var item in interviewInterviewers.ToList())
                {
                    if (!interviewerIds.Contains(item.Id))
                        await _interviewSessionInterviewerCommandRepo.DeleteAsync(item);
                }

                await _interviewSessionInterviewerCommandRepo.SaveChangesAsync();

                return new ExecutionResponse<object>
                {
                    ResponseData = true,
                    ResponseCode = ResponseCode.Ok
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<object>
                {
                    ResponseData = false,
                    ResponseCode = ResponseCode.ServerException,
                    Message = ex.Message
                };
            }
        }

        //public async Task<ExecutionResponse<object>> DeleteInterviewSession(long Id)
        //{
        //    var interviewSession = _interviewSessionQueryRepo.GetAll().FirstOrDefault(x => x.Id == Id && x.CompanyId == CurrentCompanyId);

        //    if (interviewSession == null)
        //        return new ExecutionResponse<object>
        //        {
        //            ResponseCode = ResponseCode.NotFound,
        //            Message = "No record found"
        //        };

        //    try
        //    {
        //        await _interviewSessionCommandRepo.DeleteAsync(interviewSession);
        //        await _interviewSessionCommandRepo.SaveChangesAsync();

        //        return new ExecutionResponse<object>
        //        {
        //            ResponseCode = ResponseCode.Ok,
        //            ResponseData = true
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ExecutionResponse<object>
        //        {
        //            ResponseCode = ResponseCode.ServerException,
        //            ResponseData = false
        //        };
        //    }
        //}
    }
}
