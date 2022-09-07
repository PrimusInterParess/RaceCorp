namespace RaceCorp.Services.Data.Contracts
{
    using RaceCorp.Web.ViewModels.CommonViewModels;

    public interface IGetIndexPageCategoriesList
    {
        IndexViewModel GetCategories();
    }
}
