using RaceCorp.Web.ViewModels.EventRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceCorp.Services.Data.Contracts
{
    public interface IUnregisterUserRideService
    {
        Task UnregisterUserRide(EventRegisterModel eventModel);
    }
}
