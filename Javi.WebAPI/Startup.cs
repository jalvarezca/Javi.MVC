using AutoMapper;
using Javi.BusinessLogic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Javi.WebAPI
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

            services.AddMvc()
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            //Add database context
            services.AddDbContext<Context>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            //services.AddTransient<Seeder>();
            services.AddScoped<IRepository, Repository>();

            services.AddAutoMapper();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(cfg =>
            {
                cfg.MapRoute("Default",
                    "/{controller}/{action}/{id?}",
                    new { controller = "App", Action = "Index" });      //This is the default route /App/Index Will going to the AppController and execute the Index Action
            });
        }
    }
}
