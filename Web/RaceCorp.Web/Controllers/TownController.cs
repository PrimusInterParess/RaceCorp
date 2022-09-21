namespace RaceCorp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class TownController : BaseController
    {

        ////returns all races related to the town. by townId
        public IActionResult ById(int id)
        {
            this.ViewData["id"] = $"Town id is {id}";
            return this.View();
        }
    }
}
