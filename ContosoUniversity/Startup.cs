using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ContosoUniversity.Services;
using ContosoUniversity.Repositories;
using Microsoft.IdentityModel.Logging;

namespace ContosoUniversity
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
            IdentityModelEventSource.ShowPII = true;
            services.AddDbContext<SchoolContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<ICoursesService, CoursesService>();
            services.AddTransient<IDepartmentsService, DepartmentsService>();
            services.AddTransient<IInstructorsService, InstructorsService>();
            services.AddTransient<IStudentsService, StudentsService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<,>));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllersWithViews();

            services.AddAuthentication(options =>
            {
               options.DefaultScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
               options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectDefaults.AuthenticationScheme;
            })
               .AddCookie()
               .AddOpenIdConnect(options =>
               {
                   options.Authority = "https://localhost:49155/";
                   options.ClientId = "contosouniversity";
                   options.RequireHttpsMetadata = false;
                   options.Scope.Add("profile");
                   options.Scope.Add("openid");
                   options.Scope.Add("role");
                   options.ResponseType = "id_token";
                   options.SaveTokens = true;
                   options.GetClaimsFromUserInfoEndpoint = true;
               });

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}")
                .RequireAuthorization();
            });
        }
    }
}
