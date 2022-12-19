namespace RaceCorp.Web.Areas.Identity.Pages.Account.Service
{
    using RaceCorp.Common;
    using RaceCorp.Web.Areas.Identity.Pages.Account.Service.Contracts;

    public class AccountService : IAccountService
    {
        public string GetProfilePicturePath(string gender)
        {
            if (gender == "Female")
            {
                return GlobalConstants.FemaleProfilePicturePath;
            }

            if (gender == "Male")
            {
                return GlobalConstants.MaleProfilePicturePath;
            }

            return GlobalConstants.SecretProfilePicturePath;
        }
    }
}
