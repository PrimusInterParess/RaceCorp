// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
namespace RaceCorp.Web.Areas.Identity.Pages.Account.Manage
{
#nullable disable

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Web.Areas.Identity.Pages.Account.Manage.Services;
    using RaceCorp.Web.Areas.Identity.Pages.Account.Manage.Services.Contracts;

    public class DeletePersonalDataModel : PageModel
    {
        private readonly IDeletePersonelDataService deletePersonelDataService;
        private readonly IDeletableEntityRepository<ApplicationRole> roleRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<DeletePersonalDataModel> logger;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;

        public DeletePersonalDataModel(
            IDeletePersonelDataService deletePersonelDataService,
            IDeletableEntityRepository<ApplicationRole> roleRepo,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            IDeletableEntityRepository<ApplicationUser> userRepo)
        {
            this.deletePersonelDataService = deletePersonelDataService;
            this.roleRepo = roleRepo;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.userRepo = userRepo;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            this.RequirePassword = await this.userManager.HasPasswordAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            this.RequirePassword = await this.userManager.HasPasswordAsync(user);
            if (this.RequirePassword)
            {
                if (!await this.userManager.CheckPasswordAsync(user, this.Input.Password))
                {
                    this.ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return this.Page();
                }
            }

            try
            {
                await this.deletePersonelDataService.DeleteUser(user.Id);
                await this.userRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.Page();
            }

            var userId = await this.userManager.GetUserIdAsync(user);
            var result = await this.userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }

            await this.signInManager.SignOutAsync();

            this.logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return this.Redirect("~/");
        }
    }
}
