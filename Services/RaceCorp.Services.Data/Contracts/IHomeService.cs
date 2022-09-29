namespace RaceCorp.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.CommonViewModels;

    public interface IHomeService
    {
        HomeAllViewModel GetAll(string townId, string mountainId, string formatId, string difficultyId);

        IndexViewModel GetCategories();
    }
}
