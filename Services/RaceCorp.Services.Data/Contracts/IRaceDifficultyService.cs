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
        RaceDifficultyProfileViewModel GetRaceDifficultyProfileViewModel(int raceId, int traceId);
    }
}
