namespace RaceCorp.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.DifficultyViewModels;

    public interface IRaceDifficultyService
    {
        RaceTraceProfileViewModel GetRaceDifficultyProfileViewModel(int raceId, int traceId);

        Task EditAsync(RaceTraceInputViewModel model);

        Task CreateAsync(RaceTraceInputViewModel model);

        T GetById<T>(int raceId, int traceId);
    }
}
