namespace RaceCorp.Web
{
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using RaceCorp.Data;
    using RaceCorp.Data.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Data.Repositories;
    using RaceCorp.Data.Seeding;
    using RaceCorp.Services;
    using RaceCorp.Services.Data;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Services.Messaging;
    using RaceCorp.Web.Areas.Administration.Infrastructure;
    using RaceCorp.Web.Areas.Administration.Infrastructure.Contracts;
    using RaceCorp.Web.Areas.Identity.Pages.Account.Manage.Services;
    using RaceCorp.Web.Areas.Identity.Pages.Account.Manage.Services.Contracts;
    using RaceCorp.Web.Areas.Identity.Pages.Account.Service;
    using RaceCorp.Web.Areas.Identity.Pages.Account.Service.Contracts;
    using RaceCorp.Web.Hubs;
    using RaceCorp.Web.ViewModels;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();
            Configure(app);
            app.UseAuthentication();
            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
         options =>
         {
             options.CheckConsentNeeded = context => true;
             options.MinimumSameSitePolicy = SameSiteMode.None;
         });

            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddHttpClient();
            services.AddSignalR();

            services.AddSingleton(configuration);

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            });

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(configuration["SendGrid:ApiKey"]));
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IDifficultyService, DifficultyService>();
            services.AddTransient<IFormatServices, FormatService>();
            services.AddTransient<ITownService, TownService>();
            services.AddTransient<IMountanService, MountainService>();
            services.AddTransient<IRaceService, RaceService>();
            services.AddTransient<ITraceService, TraceService>();
            services.AddTransient<IRideService, RideService>();
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<IGoogleDriveService, GoogleDriveService>();
            services.AddTransient<IGpxService, GpxService>();
            services.AddTransient<ILogoService, LogoService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IApprovalService, ApprovalService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IGroupNameProvider, GroupNameProvider>();
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<IDeletePersonelDataService, DeletePersonelDataService>();
            services.AddTransient<IRegisterUserRaceService, RegisterUserRaceService>();
            services.AddTransient<IUnregisterUserRaceService, UnregisterUserRaceService>();
            services.AddTransient<IRegisterUserRideService, RegisterUserRideService>();
            services.AddTransient<IUnregisterUserRideService, UnregisterUserRideService>();
            services.AddTransient<IJoinTeamService, JoinTeamService>();
            services.AddTransient<ILeaveTeamService, LeaveTeamService>();
            services.AddTransient<IConnectUserService, ConnectUserService>();
            services.AddTransient<IDisconnectUserService, DisconnectUserService>();
            services.AddTransient<IAdminFileService, AdminFileService>();
            services.AddTransient<IAdminRaceService, AdminRaceService>();
            services.AddTransient<IAdminRideService, AdminRideService>();
            services.AddTransient<IAdminUserService, AdminUserService>();
            services.AddTransient<IAdminContactService, AdminContactService>();
            services.AddTransient<IAccountService, AccountService>();
        }

        private static void Configure(WebApplication app)
        {
            // Seed data on application startup
            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/ErrorPage");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.MapHub<ChatHub>("/chathub");
        }
    }
}
