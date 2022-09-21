namespace RaceCorp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Services.Data.Contracts;

    public class RaceDifficultyController : BaseController
    {
        private readonly IRaceDifficultyService raceDiffService;

        public RaceDifficultyController(IRaceDifficultyService raceDiffService)
        {
            this.raceDiffService = raceDiffService;
        }

        public IActionResult RaceDifficultyProfile(int raceId, int traceId)
        {
            var model = this.raceDiffService.GetRaceDifficultyProfileViewModel(raceId, traceId);

            this.ViewData["id"] = $"Race id ={raceId}. Trace id = {traceId}";
            return this.View(model);
        }
    }
}
