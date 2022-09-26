namespace RaceCorp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Web.ViewModels.Ride;

    public class RideController : BaseController
    {
        [HttpGet]
        public IActionResult Create()
        {

        }

        [HttpPost]
        public IActionResult Create(RideCreateViewModel model)
        {

        }
    }
}
