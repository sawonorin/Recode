using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Managers
{
    public interface IVenueService
    {
        Task<ExecutionResponse<VenueModelPage>> GetVenues(string name = "", int pageSize = 10, int pageNo = 1);
        Task<ExecutionResponse<VenueModel>> GetVenue(long Id);
        Task<ExecutionResponse<object>> DeleteVenue(long Id);
        Task<ExecutionResponse<VenueModel>> CreateVenue(VenueModel model);
        Task<ExecutionResponse<VenueModel>> UpdateVenue(VenueModel model);
        Task<ExecutionResponse<VenueModel[]>> GetAvailableVenue(DateTime startTime, DateTime endTime);
    }
}
