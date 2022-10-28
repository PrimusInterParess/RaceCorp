namespace RaceCorp.Web.Areas.Administration.Infrastructure.Contracts
{
    using System.Threading.Tasks;

    using RaceCorp.Web.Areas.Administration.Models;

    public interface IAdminService
    {
        Task UploadingPicture(PictureUploadModel inputModel, string roothPath, string userId);
    }
}
