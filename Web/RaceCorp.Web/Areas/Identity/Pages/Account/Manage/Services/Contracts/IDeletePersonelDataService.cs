namespace RaceCorp.Web.Areas.Identity.Pages.Account.Manage.Services.Contracts
{
    using System.Threading.Tasks;

    using RaceCorp.Data.Models;

    public interface IDeletePersonelDataService
    {
        Task DeleteUser(string userId);
    }
}
