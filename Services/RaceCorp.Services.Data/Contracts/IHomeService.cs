namespace RaceCorp.Services.Data.Contracts
{
    using RaceCorp.Web.ViewModels.Home;

    public interface IHomeService
    {
        HomeAllViewModel GetAll(string townId, string mountainId, string formatId, string difficultyId);

        IndexViewModel GetIndexModel();
    }
}
