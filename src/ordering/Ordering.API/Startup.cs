using System.Reflection;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Producer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ordering.API.RabbitMq;
using Ordering.Application.Handlers;
using Ordering.Core.Repositories;
using Ordering.Core.Repositories.Base;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;
using Ordering.Infrastructure.Repositories.Base;
using RabbitMQ.Client;

namespace Ordering.API
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Ordering.API", Version = "v1"});
            });

            services
                .AddDbContext<OrderDbContext
                >(c =>
                  {
                      c.UseSqlServer(Configuration.GetConnectionString("OrderConnection"),
                                     a=>a.MigrationsAssembly("Ordering.API"));
                      
                  },
                  ServiceLifetime.Singleton);
         
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(typeof(CheckoutOrderHandler).GetTypeInfo().Assembly);
            services.AddSingleton<IRabbitMqConnection>(sp => new RabbitMqConnection(new ConnectionFactory
            {
                HostName = Configuration["EventBus:HostName"],
                UserName = Configuration["EventBus:UserName"],
                Password = Configuration["EventBus:Password"]
            }));
            services.AddSingleton<EventBusRabbitMqConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
            }

            app.UseRabbitMqListener();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}