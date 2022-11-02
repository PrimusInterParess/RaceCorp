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
        RaceTraceProfileModel GetRaceTraceProfileViewModel(int raceId, int traceId);

        Task EditAsync(RaceTraceEditModel model, string gxpFileRoothPath, string userId);

        Task CreateRaceTraceAsync(RaceTraceEditModel model, string roothPath, string userId);

        T GetById<T>(int raceId, int traceId);

        Task<Trace> ProccedingData(TraceInputModel traceInputModel);

        Task<bool> DeleteTraceAsync(int id);
    }
}
