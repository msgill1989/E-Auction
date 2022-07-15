using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using BuyerService.BusinessLayer.Interfaces;
using BuyerService.BusinessLayer;
using BuyerService.RepositoryLayer.Interfaces;
using BuyerService.RepositoryLayer;
using BuyerService.Models;
using Serilog;
using Confluent.Kafka;
using BuyerService.Kafka;

namespace BuyerService
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
            var consumerConfig = new ConsumerConfig();
            Configuration.Bind("consumer", consumerConfig);
            services.AddSingleton<ConsumerConfig>(consumerConfig);

            var producerConfig = new ProducerConfig();
            Configuration.Bind("producer", producerConfig);
            services.AddSingleton<ProducerConfig>(producerConfig);

            services.AddTransient<IBuyerBusinessLogic, BuyerBusinessLogic>();
            services.AddTransient<IBuyerRepository, BuyerRepository>();
            services.AddHostedService<KafkaConsumer>();

            //MongoDB settings
            services.Configure<EAuctionDatabaseSettings>(Configuration.GetSection("EAuctionDatabase"));

            services.AddMvc();
            services.AddMvcCore();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "Buyer Service API",
                    Version = "v1",
                    Description = "This is buyer service API"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Enable middleware to serve generated swagger as a json endpoint.
            app.UseSwagger();

            //Enable middleware to server swagger UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-auction buyer API");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
