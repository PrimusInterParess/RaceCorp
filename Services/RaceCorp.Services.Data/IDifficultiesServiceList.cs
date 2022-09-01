using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaceCorp.Web.ViewModels.DifficultyViewModels;

namespace RaceCorp.Services.Data
{
    public interface IDifficultiesServiceList
    {
        HashSet<DifficultyViewModel> GetDifficulties();
    }
}
