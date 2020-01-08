using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using ClientBase.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            services.AddTransient<IClientRepository, EFClientRepository>();
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
                   name: "pagination",
                   template: "Company/Page/{productPage}",
                   defaults: new { controller = "Company", Action = "List" });

                   routes.MapRoute(
                       name: "default",
                       template: "{controller=Founder}/{action=List}/{id?}");
               });

            SeedData.EnsurePopulated(app);
        }
    }
}
