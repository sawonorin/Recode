using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Recode.Core.ConfigModels;
using Recode.Core.Interfaces.Managers;
using Recode.Core.Interfaces.Repositories;
using Recode.Core.Interfaces.Services;
using Recode.Core.Managers;
using Recode.Data.MongoDB.Interfaces;
using Recode.Service.AspNetCoreHelper;
using Recode.Service.AuditHelper;
using Recode.Service.Implementations;
//using Recode.Service.Implementations.Repositories;
using Recode.Service.Implementations.Services;
using Recode.Repository.CoreRepositories;
using Recode.Service.EntityService;
using Recode.Service.SSO;
using Recode.Service.Implementations.Repositories;
using Amazon.S3;

namespace Recode.Api.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddAppServices(this IServiceCollection services,
           IConfiguration Configuration)
        {
            // Register the ConfigurationBuilder instance of AuthSettings
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
            services.Configure<APPURL>(Configuration.GetSection(nameof(APPURL)));
            services.Configure<MailSetting>(Configuration.GetSection(nameof(MailSetting)));
            services.Configure<SSoSetting>(Configuration.GetSection("SSoSetting"));

            //aspnetcore related
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IClientInfoProvider, HttpContextClientInfoProvider>();

            // jobs
            services.AddScoped<IEmailSendingJob, EmailSendingJob>();

            //Services
            services.AddScoped<ILoggerService, LoggerService>();
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IHttpContextExtensionService, HttpContextExtensionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailSendingJob, EmailSendingJob>();
            services.AddScoped<IAuditLogService, AuditLogService>();

            services.AddScoped<ISSOService, SSOService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IJobRoleService, JobRoleService>();
            services.AddScoped<IVenueService, VenueService>();
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddScoped<IMetricService, MetricService>();
            services.AddScoped<IInterviewSessionService, InterviewSessionService>();

            // Managers
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped(typeof(IRepositoryCommand<,>), typeof(RepositoryCommand<,>));
            services.AddScoped(typeof(IRepositoryQuery<,>), typeof(RepositoryQuery<,>));

            //Repository
            //services.AddScoped<IRoleRepository, RoleRepository>();
            //services.AddScoped<IPermissionRepository, PermissionRepository>();
            //services.AddScoped<ICorporateRepository, CorporateRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            //services.AddScoped<IBusinessRepository, BusinessRepository>();
            //services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
            //services.AddScoped<ICorporateDocumentRepository, CorporateDocumentRepository>();
            //services.AddScoped<IBusinessAccountRepository, BusinessAccountRepository>();
            //services.AddScoped<IBankRepository, BankRepository>();
        }

        public static void AddDocumentationServices(this IServiceCollection services,
         IConfiguration Configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "RECODE API"
                });
                //c.DescribeAllEnumsAsStrings();
                // Configure Swagger to use the xml documentation file
                var xmlFile = Path.ChangeExtension(typeof(Startup).Assembly.Location, ".xml");
                c.IncludeXmlComments(xmlFile);

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                  {
                    { "Bearer", new string[] { } }
                  });
            });

        }

        public static void AddAWSServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<AWSSettings>(Configuration.GetSection(nameof(AWSSettings)));

            var awsOption = Configuration.GetAWSOptions();
            services.AddAWSService<IAmazonS3>(awsOption);
        }
    }
}
