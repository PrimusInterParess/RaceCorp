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
            // TODO:Redirect to ProfilePage;
            return this.Redirect("/");
        }
    }
}
