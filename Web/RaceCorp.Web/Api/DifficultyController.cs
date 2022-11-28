namespace RaceCorp.Web.Api
{
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.Controllers;

    [ApiController]
    public class DifficultyController : BaseController
    {
        private readonly IDifficultyService difficultyService;

        public DifficultyController(IDifficultyService difficultyService)
        {
            this.difficultyService = difficultyService;
        }

        [HttpGet]
        [Route("api/difficulty/difficulties")]
        public IActionResult Difficulties()
        {
            return this.Json(this.difficultyService.GetDifficultiesKVP());
        }
    }
}
