namespace RaceCorp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class MountainController : BaseController
    {
        public IActionResult ById(int id)
        {
            this.ViewData["id"] = $"Mountain id: {id}";
            return this.View();
        }
    }
}
