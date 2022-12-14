namespace RaceCorp.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Models;
    using RaceCorp.Web.ViewModels.Trace;

    public interface ITraceService
    {
        Task EditAsync(RaceTraceEditModel model, string userId);

        Task CreateRaceTraceAsync(RaceTraceEditModel model, string userId);

        T GetById<T>(int raceId, int traceId);

        Task<Trace> ProccedingData(TraceInputModel traceInputModel, string gpxGoogleId);

        Task<bool> DeleteTraceAsync(int id);

        void UpdateInfo(RaceTraceProfileModel traceModel,ApplicationUser user);
    }
}
