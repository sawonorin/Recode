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
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Recode.Service.EntityService
{
    public class VenueService : IVenueService
    {
        private readonly IHttpContextExtensionService _httpContext;
        private readonly IRepositoryCommand<Venue, long> _venueCommandRepo;
        private readonly IRepositoryQuery<Venue, long> _venueQueryRepo;
        private readonly IRepositoryQuery<InterviewSession, long> _interviewSessionQueryRepo;
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

        public VenueService(
            IRepositoryCommand<Venue, long> venueCommandRepo,
            IRepositoryQuery<Venue, long> venueQueryRepo,
            IRepositoryQuery<InterviewSession, long> interviewSessionQueryRepo,
            IMapper mapper, IHttpContextExtensionService httpContext)
        {
            _venueCommandRepo = venueCommandRepo;
            _venueQueryRepo = venueQueryRepo;
            _interviewSessionQueryRepo = interviewSessionQueryRepo;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public async Task<ExecutionResponse<VenueModel>> CreateVenue(VenueModel model)
        {
            var oldVenue = _venueQueryRepo.GetAll().FirstOrDefault(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower());

            if (oldVenue != null)
                throw new Exception("Venue already exists");

            //save venue info
            var venue = new Venue
            {
                Name = model.Name,
                CompanyId = CurrentCompanyId,
                Description = model.Description,
                CreateById = _httpContext.GetCurrentSSOUserId()
            };

            await _venueCommandRepo.InsertAsync(venue);

            await _venueCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<VenueModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<VenueModel>(venue)
            };
        }

        public async Task<ExecutionResponse<VenueModel>> GetVenue(long Id)
        {
            var venue = _venueQueryRepo.GetAll().FirstOrDefault(x => x.Id == Id && x.CompanyId == CurrentCompanyId);

            if (venue == null)
                return new ExecutionResponse<VenueModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            return new ExecutionResponse<VenueModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<VenueModel>(venue)
            };
        }

        public async Task<ExecutionResponse<VenueModelPage>> GetVenues(string name = "", int pageSize = 10, int pageNo = 1)
        {
            var venues = _venueQueryRepo.GetAll().Where(x => x.CompanyId == CurrentCompanyId);

            venues = string.IsNullOrEmpty(name) ? venues : venues.Where(x => x.Name.Contains(name));

            venues.OrderBy(x => x.Name);

            venues.Skip(pageSize * (pageNo - 1)).Take(pageSize);

            return new ExecutionResponse<VenueModelPage>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = new VenueModelPage
                {
                    PageSize = pageSize,
                    PageNo = pageNo,
                    Venues = _mapper.Map<VenueModel[]>(venues.ToList())
                }
            };
        }

        public async Task<ExecutionResponse<VenueModel>> UpdateVenue(VenueModel model)
        {
            var venue = _venueQueryRepo.GetAll().FirstOrDefault(x => x.Id == model.Id && x.CompanyId == CurrentCompanyId);

            if (venue == null)
                return new ExecutionResponse<VenueModel>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };
                       
            //update venue record in db
            venue.Name = model.Name;
            venue.Description = model.Description;

            await _venueCommandRepo.UpdateAsync(venue);
            await _venueCommandRepo.SaveChangesAsync();

            return new ExecutionResponse<VenueModel>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<VenueModel>(venue)
            };
        }

        public async Task<ExecutionResponse<object>> DeleteVenue(long Id)
        {
            var venue = _venueQueryRepo.GetAll().FirstOrDefault(x => x.Id == Id && x.CompanyId == CurrentCompanyId);

            if (venue == null)
                return new ExecutionResponse<object>
                {
                    ResponseCode = ResponseCode.NotFound,
                    Message = "No record found"
                };

            try
            {
                await _venueCommandRepo.DeleteAsync(venue);
                await _venueCommandRepo.SaveChangesAsync();

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

        public async Task<ExecutionResponse<VenueModel[]>> GetAvailableVenue(DateTime startTime, DateTime endTime)
        {
            var unavailableVenueIds = _interviewSessionQueryRepo.GetAll().Where(x => (startTime >= x.StartTime && startTime <= x.EndTime) || (endTime >= x.StartTime && endTime <= x.EndTime)).Select(x=>x.VenueId).ToArray();

            var venues = _venueQueryRepo.GetAll();

            venues = unavailableVenueIds.Count() > 0 ? venues.Where(v => !unavailableVenueIds.Contains(v.Id)) : venues;

            return new ExecutionResponse<VenueModel[]>
            {
                ResponseCode = ResponseCode.Ok,
                ResponseData = _mapper.Map<VenueModel[]>(venues.ToArray())
            };
        }
    }
}
