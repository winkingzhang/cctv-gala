using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using Serilog;
using Thoughtworks.Gala.WebApi.ModelBinders;
using Thoughtworks.Gala.WebApi.Repositories;

namespace Thoughtworks.Gala.WebApi
{
    [ExcludeFromCodeCoverage]
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
            var dynamoDbConfig = Configuration.GetSection("DynamoDb");
            var runLocalDynamoDb = dynamoDbConfig.GetValue<bool>("LocalMode");

            if (runLocalDynamoDb)
            {
                services.AddSingleton<IAmazonDynamoDB>(sp =>
                {
                    var clientConfig = new AmazonDynamoDBConfig
                    {
                        ServiceURL = dynamoDbConfig.GetValue<string>("LocalServiceUrl") ?? "http://localhost:8000"
                    };
                    return new AmazonDynamoDBClient(clientConfig);
                });
            }
            else
            {
                services.AddAWSService<IAmazonDynamoDB>();
            }

            services.AddTransient<IDynamoDBContext, DynamoDBContext>();

            services.AddScoped<IGalaRepository, GalaRepository>();
            services.AddScoped<IProgramRepository, ProgramRepository>();
            services.AddScoped<IPerformerRepository, PerformerRepository>();

            services.AddHttpContextAccessor();

            services.AddControllers().AddMvcOptions(options => 
                options.ModelBinderProviders.Insert(0, new YearsModelBinderProvider()));

            // Register the Swagger Generator service. This service is responsible for genrating Swagger Documents.
            // Note: Add this service at the end after AddMvc() or AddMvcCore().
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(t => t.ToString().Replace("Thoughtworks.Gala.WebApi.", ""));

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CCTV Gala API",
                    Version = "v1",
                    Description = "A RESTful api for represent CCTV Gala",
                    Contact = new OpenApiContact
                    {
                        Name = "Thoughtworks .NET Core group",
                        Email = "noreply@tw-dotnet-group.info",
                    },
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            }).AddSwaggerGenNewtonsoftSupport();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app /*, IWebHostEnvironment env*/)
        {
            app.UseExceptionHandler("/error");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CCTV Gala API"); });

            // The default HSTS value is 30 days.
            // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            
            //app.UseHttpsRedirection();
            
            // Write streamlined request completion events, instead of the more verbose ones from the framework.
            // To use the default framework request logging instead, remove this line and set the "Microsoft"
            // level in appsettings.json to "Information".
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}