using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.Entities;

namespace WebApp
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
            services.AddDbContext<BaseCoreDataContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("ThanhSonDB")));
            services.AddIdentity<AspNetUsers, AspNetRoles>(options => options.SignIn.RequireConfirmedAccount = false)
                //.AddDefaultIdentity<IdentityUser>()
               .AddEntityFrameworkStores<BaseCoreDataContext>()
               .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
            #region Public Transient
            //Cache
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<UserManager<AspNetUsers>, UserManager<AspNetUsers>>();
            services.AddTransient<SignInManager<AspNetUsers>, SignInManager<AspNetUsers>>();
            services.AddTransient<RoleManager<AspNetRoles>, RoleManager<AspNetRoles>>();
            #endregion
            services.AddDistributedMemoryCache();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);//You can set Time   
            });
            services.AddResponseCompression();
            services.AddControllersWithViews();
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)

        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });


            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStatusCodePages(context =>
            {
                var response = context.HttpContext.Response;
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized )
                    response.Redirect("/Account/Login");
                return Task.CompletedTask;
            });
            app.UseMvc(routes =>
            {

                routes.MapRoute(
                "Product",
                "sanpham", // URL with parameters
                new { controller = "Product", action = "Showsanpham" }
             );


                routes.MapRoute(
                "Company",
                "Company", // URL with parameters
                new { controller = "Company", action = "Index"}
             );

				routes.MapRoute(
			   "Units",
			   "Units", // URL with parameters
			   new { controller = "Units", action = "Show" }
			);
				routes.MapRoute(
			   "Module",
			   "Module", // URL with parameters
			   new { controller = "Module", action = "Show" }
			);

				routes.MapRoute(
               "User",
               "User", // URL with parameters
               new { controller = "User", action = "Show" }
            );

                routes.MapRoute(
               "Brand",
               "Brand", // URL with parameters
               new { controller = "Brand", action = "ShowBrand"}
            );

                routes.MapRoute(
              "Category",
              "Category", // URL with parameters
              new { controller = "Category", action = "ShowCategory" }
           );
				routes.MapRoute(
		   "Roles",
		   "Roles", // URL with parameters
		   new { controller = "Roles", action = "Show" }
		);


				routes.MapRoute(
                    name: "default",
                    template: "{controller=Product}/{action=index}/{id?}");

            });
        }
    }
}
