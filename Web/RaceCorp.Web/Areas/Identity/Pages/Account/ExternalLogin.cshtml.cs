// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
namespace RaceCorp.Web.Areas.Identity.Pages.Account
{
#nullable disable

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using RaceCorp.Common;
    using RaceCorp.Data;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Messaging;
    using RaceCorp.Web.Areas.Identity.Pages.Account.Dtos;
    using RaceCorp.Web.Areas.Identity.Pages.Account.Service.Contracts;

    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStore<ApplicationUser> userStore;
        private readonly IUserEmailStore<ApplicationUser> emailStore;
        private readonly IEmailSender emailSender;
        private readonly IDeletableEntityRepository<ApplicationRole> roleRepo;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        private readonly IAccountService accountService;
        private readonly ILogger<ExternalLoginModel> logger;

        public ExternalLoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            IDeletableEntityRepository<ApplicationRole> roleRepo,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IAccountService accountService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userStore = userStore;
            this.emailStore = this.GetEmailStore();
            this.logger = logger;
            this.emailSender = emailSender;
            this.roleRepo = roleRepo;
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
            this.accountService = accountService;
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
        public string ProviderDisplayName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

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
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGet() => this.RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = this.Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = this.signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");
            if (remoteError != null)
            {
                this.ErrorMessage = $"Error from external provider: {remoteError}";
                return this.RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var info = await this.signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                this.ErrorMessage = "Error loading external login information.";
                return this.RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await this.signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                this.logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return this.LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return this.RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                this.ReturnUrl = returnUrl;
                this.ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    this.Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                    };
                }

                return this.Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await this.signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                this.ErrorMessage = "Error loading external login information during confirmation.";
                return this.RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            string pictureUri = string.Empty;

            if (info.LoginProvider.ToLower() == "google")
            {
                var httpClient = this.httpClientFactory.CreateClient();
                string peopleApiKey = this.configuration["Authentication:Google:ApiKey"];
                var googleAccountId = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                var photosResponse = await httpClient.GetFromJsonAsync<PeopleApiPhotos>($"https://people.googleapis.com/v1/people/{googleAccountId}?personFields=photos&key={peopleApiKey}");

                pictureUri = photosResponse?.photos.FirstOrDefault()?.url;
            }

            var firstName = info.Principal.FindFirst(ClaimTypes.GivenName).Value;
            var lastName = info.Principal.FindFirst(ClaimTypes.Surname).Value;

            if (this.ModelState.IsValid)
            {
                var user = this.CreateUser();

                if (string.IsNullOrEmpty(pictureUri))
                {
                    pictureUri = this.accountService.GetProfilePicturePath("Secret");
                }

                await this.userStore.SetUserNameAsync(user, this.Input.Email, CancellationToken.None);
                await this.emailStore.SetEmailAsync(user, this.Input.Email, CancellationToken.None);

                IdentityResult result;

                try
                {
                    result = await this.userManager.CreateAsync(user);
                }
                catch (Exception)
                {
                    return this.RedirectToAction("ErrorPage", "Home", new { area = string.Empty });
                }

                if (result.Succeeded)
                {
                    if (firstName != null)
                    {
                        user.FirstName = firstName;
                    }

                    if (lastName != null)
                    {
                        user.LastName = lastName;
                    }

                    if (firstName != null && lastName != null)
                    {
                        await this.userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, $"{firstName} {lastName}"));
                    }

                    user.ProfilePicturePath = pictureUri;
                    var roleId = this.roleRepo.All().FirstOrDefault(r => r.Name == GlobalConstants.UserRoleName)?.Id;
                    await this.userManager.AddToRoleAsync(user, GlobalConstants.UserRoleName);

                    user.Roles = new List<IdentityUserRole<string>>()
                {
                    new IdentityUserRole<string>() { RoleId = roleId },
                };

                    result = await this.userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        this.logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await this.userManager.GetUserIdAsync(user);
                        var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = this.Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: this.Request.Scheme);

                        try
                        {
                            await this.emailSender.SendEmailAsync(
                                "diesonnekind@gmail.com",
                                "Dani from RaceCorp",
                                this.Input.Email,
                                "Email confirmation",
                                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return this.RedirectToPage("./RegisterConfirmation", new { Email = this.Input.Email });
                        }

                        await this.signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            this.ProviderDisplayName = info.ProviderDisplayName;
            this.ReturnUrl = returnUrl;
            return this.Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!this.userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<ApplicationUser>)this.userStore;
        }
    }
}
