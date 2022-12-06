namespace RaceCorp.Web.Areas.Administration.Infrastructure.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RaceCorp.Web.Areas.Administration.Models;
    using RaceCorp.Web.Areas.Administration.Models.Admin;
    using RaceCorp.Web.Areas.Administration.Models.Message;
    using RaceCorp.Web.ViewModels.Administration.Dashboard;

    public interface IAdminService
    {
        Task UploadingPicture(PictureUploadModel inputModel, string roothPath, string userId);

        DashboardIndexViewModel GetIndexModel();

        ICollection<AdminContactMessage> GetMessages();

        MessageProfileModel GetMessage(int id);

        Task SaveReply(MessageProfileModel inputModel);
    }
}
