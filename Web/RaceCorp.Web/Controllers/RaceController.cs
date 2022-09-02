namespace RaceCorp.Web.Controllers
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Web.ViewModels.RaceViewModels;

    public class RaceController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Add(AddRaceInputViewModel model)
        {
            int raceId = 0;

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            // TODO:Redirect to ProfilePage;

            return this.RedirectToAction(nameof(RaceController.RaceProfile), nameof(RaceController), new { raceId = raceId });
        }

        public IActionResult RaceProfile(int raceId)
        {
            return this.View();
        }
    }
}
