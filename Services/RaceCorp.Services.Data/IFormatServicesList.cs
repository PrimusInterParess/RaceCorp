namespace RaceCorp.Services.Data
{
    using System.Collections.Generic;

    using RaceCorp.Web.ViewModels.FormatViewModels;

    public interface IFormatServicesList
    {
        HashSet<FormatViewModel> GetFormats();
    }
}
