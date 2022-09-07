namespace RaceCorp.Web.Controllers
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.RaceViewModels;

    public class RaceController : Controller
    {
        private readonly IFormatServices formatsList;

        public RaceController(IFormatServices formatsList)
        {
            this.formatsList = formatsList;
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new AddRaceInputViewModel()
            {
                Formats = this.formatsList.GetFormatKVP(),
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Add(AddRaceInputViewModel model)
        {
            // TODD: validate Data!
            int raceId = 0;

            if (!this.ModelState.IsValid)
            {
                model.Formats = this.formatsList.GetFormatKVP();
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
