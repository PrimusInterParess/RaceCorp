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
            return this.RedirectToAction("RaceProfile", model);
        }

        [HttpGet]
        public IActionResult RaceProfile(AddRaceInputModel model)
        {
            return this.View(model);
        }
    }
}
