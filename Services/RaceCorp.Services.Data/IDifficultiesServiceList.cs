namespace RaceCorp.Services.Data
{
    using System.Collections.Generic;

    using RaceCorp.Web.ViewModels.DifficultyViewModels;

    public interface IDifficultiesServiceList
    {
        HashSet<DifficultyViewModel> GetDifficulties();
    }
}
