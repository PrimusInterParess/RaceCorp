namespace RaceCorp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Home;
    using static RaceCorp.Services.Constants.Common;

    public class HomeService : IHomeService
    {
        private readonly IDeletableEntityRepository<Race> raceRepo;
        private readonly IDeletableEntityRepository<Ride> rideRepo;
        private readonly IDifficultyService getDifficultiesServiceList;
        private readonly IFormatServices formatServicesList;
        private readonly ITownService townService;
        private readonly IMountanService mountanService;
        private readonly IRaceService raceService;
        private readonly IRepository<Image> imageRepo;

        public HomeService(
            IDeletableEntityRepository<Race> raceRepo,
            IDeletableEntityRepository<Ride> rideRepo,
            IDifficultyService getDifficultiesServiceList,
            IFormatServices formatServicesList,
            ITownService townService,
            IMountanService mountanService,
            IRaceService raceService,
            IRepository<Image> imageRepo)
        {
            this.raceRepo = raceRepo;
            this.rideRepo = rideRepo;
            this.getDifficultiesServiceList = getDifficultiesServiceList;
            this.formatServicesList = formatServicesList;
            this.townService = townService;
            this.mountanService = mountanService;
            this.raceService = raceService;
            this.imageRepo = imageRepo;
        }

        public HomeAllViewModel GetAll(string townId, string mountainId, string formatId, string difficultyId)
        {
            throw new NotImplementedException();
        }

        public IndexViewModel GetIndexModel()
        {
            var townImage = this.imageRepo
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefault(x => x.Name == TownImageName);

            var mountainImage = this.imageRepo
                .AllAsNoTracking()
                                .OrderByDescending(x => x.CreatedOn)

                .FirstOrDefault(x => x.Name == MountainImageName);

            var upcommingRaceImage = this.imageRepo
                .AllAsNoTracking()
                                .OrderByDescending(x => x.CreatedOn)

                .FirstOrDefault(x => x.Name == UpcommingRaceImageName);

            var upcommingRidesImage = this.imageRepo
                .AllAsNoTracking()
                                .OrderByDescending(x => x.CreatedOn)

                .FirstOrDefault(x => x.Name == UpcommingRidesImageName);

            var model = new IndexViewModel();

            if (townImage != null)
            {
                model.TownImagePath = SystemRoothPath + townImage.Id + "." + townImage.Extension;
            }

            if (mountainImage != null)
            {
                model.MountainImagePath = SystemRoothPath + mountainImage.Id + "." + mountainImage.Extension;
            }

            if (upcommingRaceImage != null)
            {
                model.UpcomingRaceImagePath = SystemRoothPath + upcommingRaceImage.Id + "." + upcommingRaceImage.Extension;
            }

            if (upcommingRidesImage != null)
            {
                model.UpcomingRidesImagePath = SystemRoothPath + upcommingRidesImage.Id + "." + upcommingRidesImage.Extension;
            }

            return model;
        }
    }
}
