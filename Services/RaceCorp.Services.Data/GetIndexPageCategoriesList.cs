namespace RaceCorp.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.CommonViewModels;

    public class GetIndexPageCategoriesList : IGetIndexPageCategoriesList
    {
        private readonly IDifficultyService getDifficultiesServiceList;
        private readonly IFormatServices formatServicesList;
        private readonly ITownService townService;
        private readonly IMountanService mountanService;

        public GetIndexPageCategoriesList(
            IDifficultyService getDifficultiesServiceList,
            IFormatServices formatServicesList,
            ITownService townService,
            IMountanService mountanService)
        {
            this.getDifficultiesServiceList = getDifficultiesServiceList;
            this.formatServicesList = formatServicesList;
            this.townService = townService;
            this.mountanService = mountanService;
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
