namespace RaceCorp.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.CommonViewModels;

    public interface IMountanService
    {
        HashSet<MountainViewModel> GetMountains();

        IEnumerable<KeyValuePair<string, string>> GetMountainsKVP();
    }
}
