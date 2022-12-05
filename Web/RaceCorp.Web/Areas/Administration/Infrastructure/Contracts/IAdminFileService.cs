namespace RaceCorp.Web.Areas.Administration.Infrastructure.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IAdminFileService
    {
        Task ProccessingImageData(IFormFile file, string imageName, string userId, string roothPath, string childrenFolderName);
    }
}
