using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReversiMvcApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.ReCaptcha;

namespace ReversiMvcApp
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddReCaptcha(Configuration.GetSection("ReCaptcha"));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDbContext<ReversiDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ReversiDB")), ServiceLifetime.Singleton);

            services.AddCors(
                options =>
                {
                    options.AddPolicy(
                        name: MyAllowSpecificOrigins,
                        builder =>
                        {
                            builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        }
                    );
                }
            );

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddTransient<APIService>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            MakeRole("Player", roleManager).GetAwaiter().GetResult();
            MakeRole("Beheerder", roleManager).GetAwaiter().GetResult();
            MakeRole("Mediator", roleManager).GetAwaiter().GetResult();

            MakeBeheerder(userManager).GetAwaiter().GetResult();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private async Task MakeRole(string roleName, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                return;
            }
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        private async Task MakeBeheerder(UserManager<IdentityUser> userManager)
        {
            var user = await userManager.FindByEmailAsync("beheerder@hotmail.com");

            if (user != null)
            {
                return;
            }

            var beheerder = new IdentityUser { UserName = "beheerder@hotmail.com", Email = "beheerder@hotmail.com" };
            await userManager.CreateAsync(beheerder, "Test123!");
            await userManager.AddToRoleAsync(beheerder, "Beheerder");
        }

    }
}
