namespace RaceCorp.Web.Controllers
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Web.InputViewModels;

    public class RaceController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Add(AddRaceInputModel model)
        {
            return this.View(model);
        }

        [HttpGet]
        public IActionResult RaceProfile(RaceProfileViewModel model)
        {
            return this.View(model);
        }
    }
}
