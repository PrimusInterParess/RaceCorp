namespace RaceCorp.Web.Areas.Administration.Infrastructure
{
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.Areas.Administration.Infrastructure.Contracts;
    using RaceCorp.Web.Areas.Administration.Models;

    using static RaceCorp.Services.Constants.Common;

    public class AdminService : IAdminService
    {
        private readonly IFileService fileService;
        private readonly IRepository<Image> imageRepo;
        private readonly IAdminRideService adminRideService;
        private readonly IAdminRaceService adminRaceService;
        private readonly IRaceService raceService;

        public AdminService(
            IFileService fileService,
            IRepository<Image> imageRepo,
            IAdminRideService adminRideService,
            IAdminRaceService adminRaceService,
            IRaceService raceService)
        {
            this.fileService = fileService;
            this.imageRepo = imageRepo;
            this.adminRideService = adminRideService;
            this.adminRaceService = adminRaceService;
            this.raceService = raceService;
        }

        public DashboardIndexViewModel GetIndexModel()
        {
            return new DashboardIndexViewModel
            {
                NoOwnerRaces = this.adminRaceService.GetNoOwnerRaces(),
                NoOwnerRides = this.adminRideService.GetNoOwnerRides(),
            };
        }

        public async Task UploadingPicture(PictureUploadModel inputModel, string roothPath, string userId)
        {
            var image = await this.fileService.ProccessingImageData(inputModel.Picture, userId, roothPath, SystemImageFolderName);
            image.Name = inputModel.Type;

            await this.imageRepo.SaveChangesAsync();
        }
    }
}
