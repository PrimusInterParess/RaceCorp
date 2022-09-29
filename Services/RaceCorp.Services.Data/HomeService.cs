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
    using RaceCorp.Web.ViewModels.CommonViewModels;

    public class HomeService : IHomeService
    {
        private readonly IDeletableEntityRepository<Race> raceRepo;
        private readonly IDeletableEntityRepository<Ride> rideRepo;
        private readonly IDifficultyService getDifficultiesServiceList;
        private readonly IFormatServices formatServicesList;
        private readonly ITownService townService;
        private readonly IMountanService mountanService;

        public HomeService(
            IDeletableEntityRepository<Race> raceRepo,
            IDeletableEntityRepository<Ride> rideRepo,
            IDifficultyService getDifficultiesServiceList,
            IFormatServices formatServicesList,
            ITownService townService,
            IMountanService mountanService)
        {
            this.raceRepo = raceRepo;
            this.rideRepo = rideRepo;
            this.getDifficultiesServiceList = getDifficultiesServiceList;
            this.formatServicesList = formatServicesList;
            this.townService = townService;
            this.mountanService = mountanService;
        }

        public HomeAllViewModel GetAll(string townId, string mountainId, string formatId, string difficultyId)
        {
            throw new NotImplementedException();
        }

        public IndexViewModel GetCategories()
        {
            return new IndexViewModel
            {
                Towns = this.townService.GetTownsKVP(),
                Mountains = this.mountanService.GetMountainsKVP(),
                Formats = this.formatServicesList.GetFormatKVP(),
                Difficulties = this.getDifficultiesServiceList.GetDifficultiesKVP(),
            };
        }
    }
}
