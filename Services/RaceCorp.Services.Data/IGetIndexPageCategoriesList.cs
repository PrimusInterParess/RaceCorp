namespace RaceCorp.Services.Data
{
    using RaceCorp.Web.ViewModels.HomeViewModels;

    public interface IGetIndexPageCategoriesList
    {
        IndexViewModel GetCategories();
    }
}
