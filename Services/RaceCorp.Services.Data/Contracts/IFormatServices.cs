namespace RaceCorp.Services.Data.Contracts
{
    using System.Collections.Generic;

    using RaceCorp.Web.ViewModels.Common;

    public interface IFormatServices
    {
        HashSet<FormatViewModel> GetFormats();

        IEnumerable<KeyValuePair<string, string>> GetFormatKVP();
    }
}
