﻿namespace RaceCorp.Services.Data.Contracts
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
        RaceTraceProfileModel GetRaceDifficultyProfileViewModel(int raceId, int traceId);

        Task EditAsync(RaceTraceEditModel model);

        Task CreateAsync(RaceTraceEditModel model);

        T GetById<T>(int raceId, int traceId);

        Trace GetTraceDbModel(TraceInputModel traceInputModel, Gpx gpx);
    }
}