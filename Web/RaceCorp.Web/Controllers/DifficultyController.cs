namespace RaceCorp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class DifficultyController : BaseController
    {
        public IActionResult ById(int id)
        {
            this.ViewData["id"] = id;
            return this.View();
        }
    }
}
