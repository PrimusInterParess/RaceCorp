namespace RaceCorp.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RaceCorp.Common;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Common;

    public class ApprovalController : BaseController
    {
        private readonly IApprovalService approvalService;

        public ApprovalController(IApprovalService approvalService)
        {
            this.approvalService = approvalService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ApproveRequest(ApproveRequestModel model)
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
            }

            try
            {
                await this.approvalService.ProccesApproval(model);
                return this.RedirectToAction("Requests", "User", new { area = string.Empty, id = currentUserId });
            }
            catch (Exception e)
            {
                this.TempData["ErrorMessage"] = e.Message;

                if (e.GetType() == typeof(ArgumentException))
                {
                    return this.RedirectToAction("Index", "Home", new { area = string.Empty });
                }

                return this.RedirectToAction("Requests", "User", new { area = string.Empty, id = currentUserId });
            }
        }
    }
}
