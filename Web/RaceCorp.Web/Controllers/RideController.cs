namespace RaceCorp.Web.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Ride;

    public class RideController : BaseController
    {
        private readonly IRideService rideService;
        private readonly IDifficultyService difficultyService;
        private readonly IFormatServices formatServices;

        public RideController(IRideService rideService, IDifficultyService difficultyService, IFormatServices formatServices)
        {
            this.rideService = rideService;
            this.difficultyService = difficultyService;
            this.formatServices = formatServices;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new RideCreateViewModel()
            {
                Formats = this.formatServices.GetFormatKVP(),
                DifficultiesKVP = this.difficultyService.GetDifficultiesKVP(),
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Create(RideCreateViewModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                model.Formats = this.formatServices.GetFormatKVP();
                model.DifficultiesKVP = this.difficultyService.GetDifficultiesKVP();
                return this.View(model);
            }

            return this.RedirectToAction(nameof(RideController), nameof(this.All));
        }

        [HttpGet]
        public IActionResult All()
        {
            return this.View();
        }
    }
}
