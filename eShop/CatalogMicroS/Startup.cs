using CatalogMicroS.DL;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Threading.Tasks;

namespace CatalogMicroS
{
    public class Startup
    {
        private IEventBus _eventBus;
        private IServiceScopeFactory _scopeFactory;

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


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<CatalogContext>(option => option.UseSqlServer(Configuration.GetConnectionString("Default")));
            //register interfaces to DI
            services.AddScoped<ICatalogRepository, CatalogRepository>();


            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBusConnection"]
                };

                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, retryCount);
            });

            services.AddSingleton<IEventBus, EventBusRabbitMQ.RabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                }
                var queueName = Configuration["QueueName"];

                return new EventBusRabbitMQ.RabbitMQ(rabbitMQPersistentConnection, new ProcessResult(ProcessTheResult), queueName, retryCount);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IEventBus eventBus)
        {
            _eventBus = eventBus;

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
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            _eventBus.Subscribe("Catalog");

            var provider = app.ApplicationServices;
            _scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
        }

        private async Task ProcessTheResult(string message)
        {
            dynamic json = JsonConvert.DeserializeObject(message);

            int actionValue = json.Action;

            Action enumResult = (Action)actionValue;

            if (enumResult == Action.GetAll)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var catalogRepository = scope.ServiceProvider.GetRequiredService<ICatalogRepository>();
                    var jsonToBe = await catalogRepository.GetAllItems();

                    _eventBus.Publish(jsonToBe, "Order");
                }
            }
        }
    }
}
