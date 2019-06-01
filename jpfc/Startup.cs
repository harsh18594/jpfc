using jpfc.Data;
using jpfc.Data.Interfaces;
using jpfc.Models;
using jpfc.Services;
using jpfc.Services.Interfaces;
using jpfc.ValidationAttributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace jpfc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            // hd.20190331 - removing default identity
            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                // lockout settings
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                o.Lockout.MaxFailedAccessAttempts = 3;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            AddServices(services);

            // add mvc
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Custom validation attributes
            services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidatiomAttributeAdapterProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/Index");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void AddServices(IServiceCollection services)
        {
            // services
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IServicesService, ServicesService>();
            services.AddScoped<IKaratService, KaratService>();
            services.AddScoped<IScheduledTaskService, ScheduledTaskService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IClientIdentificationService, ClientIdentificationService>();
            services.AddScoped<IClientReceiptService, ClientReceiptService>();

            // repos
            services.AddScoped<IAccessCodeRepository, AccessCodeRepository>();
            services.AddScoped<IKaratRepository, KaratRepository>();
            services.AddScoped<IPriceRepository, PriceRepository>();
            services.AddScoped<IMetalRepository, MetalRepository>();
            services.AddScoped<IIdentificationDocumentRepository, IdentificationDocumentRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IClientBelongingRepository, ClientBelongingRepository>();
            services.AddScoped<IClientIdentificationRepository, ClientIdentificationRepository>();
            services.AddScoped<IClientReceiptRepository, ClientReceiptRepository>();
        }
    }
}
