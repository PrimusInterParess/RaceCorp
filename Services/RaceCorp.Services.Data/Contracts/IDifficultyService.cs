namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;

    using RaceCorp.Web.ViewModels.DifficultyViewModels;

    public interface IDifficultyService
    {
        HashSet<DifficultyViewModel> GetDifficulties();

        IEnumerable<KeyValuePair<string, string>> GetDifficultiesKVP();
    }
}
