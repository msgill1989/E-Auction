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
using SellerService.BusinessLayer.Interfaces;
using SellerService.BusinessLayer;
using SellerService.RepositoryLayer.Interfaces;
using SellerService.RepositoryLayer;
using Serilog;
using SellerService.Models;
using Confluent.Kafka;

namespace SellerService
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
            var producerConfig = new ProducerConfig();
            Configuration.Bind("producer", producerConfig);
            services.AddSingleton<ProducerConfig>(producerConfig);

            var consumerConfig = new ConsumerConfig();
            Configuration.Bind("consumer", consumerConfig);
            services.AddSingleton<ConsumerConfig>(consumerConfig);

            services.AddTransient<ISellerBusinessLogic, SellerBusinessLogic>();
            services.AddTransient<ISellerRepository, SellerRepository>();

            //Adding MongoDB settings
            services.Configure<EAuctionDatabaseSettings>(Configuration.GetSection("EAuctionDatabase"));

            services.AddMvc();
            services.AddMvcCore();
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "Seller Service API",
                    Version = "v1",
                    Description = "This is seller service API"
                });
            });
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "E - Auction seller API");
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
