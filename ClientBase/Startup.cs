using System.Text.Encodings.Web;
using System.Text.Unicode;
using ClientBase.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.WebEncoders;

namespace ClientBase
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ClientsDatabase")));
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddTransient<IEntityRepository<Company>, EFCompanyRepository>();
            services.AddTransient<IEntityRepository<Founder>, EFFounderRepository>();
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages()
               .UseHttpsRedirection()
               .UseStaticFiles()
               .UseMvc(routes =>
               {
                   routes.MapRoute(
                     name: null,
                     template: "{controller:regex(^Company$|Founder$)}/List/page{pageNumber:int}",
                     defaults: new { controller = "Company", action = "List", pageNumber = 1 });

                   routes.MapRoute(
                        name: null,
                        template: "{controller:regex(^Company$|Founder$)}/List/{id?}",
                        defaults: new { controller = "Company", action="List"});

                   routes.MapRoute(
                       name: null,
                       template: "",
                       defaults: new { controller = "Home", action = "Index" });

                   routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
               });

            SeedData.EnsurePopulated(app);
        }
    }
}
