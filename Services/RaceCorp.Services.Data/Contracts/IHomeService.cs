namespace RaceCorp.Services.Data.Contracts
{
    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.CommonViewModels;

    public interface IHomeService
    {
        HomeAllViewModel GetAll(string townId, string mountainId, string formatId, string difficultyId);

        IndexViewModel GetIndexModel();
    }
}
