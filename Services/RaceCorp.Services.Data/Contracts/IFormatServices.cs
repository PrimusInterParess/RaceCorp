namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;

    using RaceCorp.Web.ViewModels.FormatViewModels;

    public interface IFormatServices
    {
        HashSet<FormatViewModel> GetFormats();

        IEnumerable<KeyValuePair<string, string>> GetFormatKVP();
    }
}
