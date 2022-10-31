namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserBaseModel : IMapTo<ApplicationUser>
    {
        public int Id { get; set; }

        public string UserName { get; set; }

    }
}
