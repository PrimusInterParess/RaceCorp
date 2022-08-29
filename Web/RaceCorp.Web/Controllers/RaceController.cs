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
            var modelData = new RaceProfileViewModel()
            {
                Name = model.Name,
                Location = model.Location,
                TrackUrl = model.TrackUrl,
                Length = model.Length,
                Date = model.Date,

            };

            return this.RedirectToAction(nameof(RaceController.RaceProfile),model);
        }

        [HttpGet]
        public IActionResult RaceProfile(RaceProfileViewModel model)
        {
            return this.View(model);
        }
    }
}
