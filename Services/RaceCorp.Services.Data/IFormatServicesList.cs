using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaceCorp.Web.ViewModels.FormatViewModels;

namespace RaceCorp.Services.Data
{
    public interface IFormatServicesList
    {
        HashSet<FormatViewModel> GetFormats();
    }
}
