namespace RaceCorp.Web.Areas.Administration.Infrastructure.Contracts
{
    using System.Threading.Tasks;

    using RaceCorp.Web.Areas.Administration.Models;
    using RaceCorp.Web.ViewModels.Administration.Dashboard;

    public interface IAdminService
    {
        Task UploadingPicture(PictureUploadModel inputModel, string roothPath, string userId);

        DashboardIndexViewModel GetIndexModel();

    }
}
