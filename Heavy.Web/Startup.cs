using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Heavy.Web.Data;
using Heavy.Web.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Heavy.Web.Data.Auth;
using Microsoft.AspNetCore.Authorization;

namespace Heavy.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
                    Configuration.GetConnectionString("DefaultConnection")
                    ));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
             {
                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequiredLength = 1;
                 options.Password.RequireLowercase = false;
                 options.Password.RequireUppercase = false;
             })
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<HeavyContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                });

            services.AddScoped<IAlbumService, AlbumEfService>();

            //policy授权策略注册
            services.AddAuthorization(options=> {
                options.AddPolicy("仅限管理员", policy => policy.RequireRole("administrators"));
                options.AddPolicy("编辑专辑", policy => policy.RequireClaim("Edit album", "Edit album"));
                //自定义权限认证
                options.AddPolicy("编辑专辑1", policy => policy.RequireAssertion(context =>
                {
                    if (context.User.HasClaim(x => x.Value != "Edit album"))
                        return true;
                    return false;
                }));

                //自定义AddRequirements
                options.AddPolicy("编辑专辑2", policy => policy.AddRequirements(
                    //必须全部满足
                   // new EmailRequirement("@163.com"),
                   new QualifiedUserRequirement()
                    ));
            });
            services.AddSingleton<IAuthorizationHandler, EmailHandel>();
            services.AddSingleton<IAuthorizationHandler, AdministratorsHandel >();
            services.AddSingleton<IAuthorizationHandler, CanEditAlbumHandel >();

            services.AddAntiforgery(options =>
            {
                //生成隐藏字段名
                options.FormFieldName = "AntiforgeryFieldname";
                //请求头
                options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
                options.SuppressXFrameOptionsHeader = false;
            });

            services.AddMvc(options =>
            {
                //所有action都使用验证
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });


        }



        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
    }
}
