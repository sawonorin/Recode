using System.IO;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;
using StackifyLib;
using StackifyLib.CoreLogger;
using Recode.Core.Interfaces.Services;
using Recode.Api.Extensions;
using Recode.Api.Filters;
using Recode.Api.HangfireSettingHelper;
using Recode.Api.Middlewares;
using Recode.Service.AspNetCoreHelper;
using Recode.Service.AutoMapperProfile;

namespace Recode.Api
{
    internal static class AbpMvcOptionsExtensions
    {
        public static void AddFilterOptions(this MvcOptions options, IServiceCollection services)
        {
            AddActionFilters(options);
        }

        private static void AddActionFilters(MvcOptions options)
        {
            options.Filters.AddService(typeof(APIClientRequestFilter));
        }
    }

    public class Startup
    {
        private string ContentRootPath;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            ContentRootPath = env.ContentRootPath;
            var config = builder.Build();
            Configuration = config;
            ConfigurationRoot = config;

            StackifyLib.Config.Environment = env.EnvironmentName; //optional
        }

        public IConfiguration Configuration { get; }
        public IConfigurationRoot ConfigurationRoot { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddCors();
            services.AddAutoMapper(config => config.AddProfile<MapperProfile>());
            services.AddHttpClient();
            services.AddDatabaseServices(Configuration);
            services.AddSecurityServices(Configuration);
            services.AddAppServices(Configuration);
            services.AddDocumentationServices(Configuration);
            services.AddMvc(config =>
                {
                    config.Filters.Add(new GlobalExceptionFilter());
                    config.OutputFormatters.RemoveType<TextOutputFormatter>();
                    config.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
                }).AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Formatting = Formatting.Indented;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            env.ConfigureNLog("Nlog.config");

            loggerFactory.AddStackify(); //add the provider
            app.ConfigureStackifyLogging(ConfigurationRoot); //configure settings and ASP.NET exception hooks

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                //app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            //add NLog to ASP.NET Core
            loggerFactory.AddNLog();
            app.UseMiddleware(typeof(AuthMiddleware));
            app.UseCors(x => x
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());
            app.UseExceptionHandler(builder =>
            {
                builder.Run(
                    async context =>
                    {
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        var exception = error.Error;

                        var logger = context.RequestServices.GetService<ILoggerService>();
                        logger.Error(exception);

                        var result = GlobalExceptionFilter.GetStatusCode<object>(exception);
                        context.Response.StatusCode = (int)result.statusCode;
                        context.Response.ContentType = "application/json";

                        var responseJson = JsonConvert.SerializeObject(result.responseModel, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                        await context.Response.WriteAsync(responseJson);
                    });
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            //This line enables the app to use Swagger, with the configuration in the ConfigureServices method.
            app.UseSwagger();
            //This line enables Swagger UI, which provides us with a nice, simple UI with which we can view our API calls.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RECODE API");
                c.RoutePrefix = string.Empty;
            });
            app.UseMvc();
            app.UseAuthentication();
            //The following line is also optional, if you required to monitor your jobs.
            //Make sure you're adding required authentication 
            app.UseHangfireDashboard();
            //app.UseHangfireDashboard("/appHangfire", new DashboardOptions
            //{
            //   Authorization= new[] {new HangfireDasnBoardAuthorizationFilter()}
            //});
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                WorkerCount = 1,
            });
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 5 });
            HangfireJobScheduler.SchedulerRecurringJobs();
        }
    }
}
