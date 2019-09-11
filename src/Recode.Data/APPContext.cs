
using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Recode.Data.EntityContract;
using Recode.Data.EntityBase;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;
using Recode.Data.AppEntity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System.Net;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Recode.Data
{
    public class APPContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMongoDatabase _database = null;

        #region MyDBSetRegion
        public DbSet<EmailLog> EmailLogs { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<InterviewSession> InterviewSessions { get; set; }
        public DbSet<InterviewSessionCandidate> InterviewSessionCandidates { get; set; }
        public DbSet<InterviewSessionMetric> InterviewSessionMetrics { get; set; }
        public DbSet<InterviewSessionResult> InterviewSessionResults { get; set; }
        public DbSet<InterviewSessionInterviewer> InterviewSessionInterviewers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<JobRole> JobRoles { get; set; }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Company> Comapanies { get; set; }
        #endregion

        public APPContext(DbContextOptions<APPContext> options)
          : base(options)
        {
           // _httpContextAccessor = (IHttpContextAccessor)this.GetInfrastructure().GetService(typeof(IHttpContextAccessor)); ;
        }

        private IMongoCollection<AuditLog> AuditLog
        {
            get
            {
                return _database.GetCollection<AuditLog>("AuditLog");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().HasIndex(x => x.CompanyId);
            builder.Entity<User>().HasIndex(x => x.SSOUserId).IsUnique();
            builder.Entity<Department>().HasIndex(x => x.CompanyId);
            builder.Entity<JobRole>().HasIndex(x => x.DepartmentId);
            builder.Entity<Candidate>().HasIndex(x => x.CompanyId);
            builder.Entity<InterviewSession>().HasIndex(x => x.CompanyId);
            builder.Entity<InterviewSession>().HasIndex(x => x.DepartmentId);
            builder.Entity<InterviewSession>().HasIndex(x => x.JobRoleId);
            builder.Entity<InterviewSession>().HasIndex(x => x.RecruiterId);
            builder.Entity<InterviewSessionCandidate>().HasIndex(x => x.CandidateId);
            builder.Entity<InterviewSessionCandidate>().HasIndex(x => x.InterviewSessionId);
            builder.Entity<InterviewSessionMetric>().HasIndex(x => x.MetricId);
            builder.Entity<InterviewSessionMetric>().HasIndex(x => x.InterviewSessionId);
            builder.Entity<Metric>().HasIndex(x => x.CompanyId);
            builder.Entity<InterviewSessionInterviewer>().HasIndex(x => x.InterviewSessionId);
            builder.Entity<InterviewSessionInterviewer>().HasIndex(x => x.InterviewerId);
            builder.Entity<InterviewSessionResult>().HasIndex(x => x.InterviewSessionId);
            builder.Entity<InterviewSessionResult>().HasIndex(x => x.CandidateId);
            builder.Entity<InterviewSessionResult>().HasIndex(x => x.MetricId);

            SeedRoles(builder);
        }

        #region MyOverRideSaveChangeRegion
        public override int SaveChanges()
        {

            try
            {
                // Audits();
                return base.SaveChanges();
            }
            catch { return 0; }

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                // Audits();
                return await base.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException filterContext)
            {
                Debug.WriteLine("Concurrency Error: {0}", filterContext.Message);
                return await Task.FromResult(0);

            }

        }
        #endregion

        #region MyDateCreated&DateUpdateRegion
        private void Audits()
        {
            var entities = ChangeTracker.Entries().Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified));
            long userId = 0;
            try
            {
                userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                foreach (var entity in entities)
                {
                    foreach (var log in GetAuditRecordsForChange(entity, userId))
                    {
                        if (log != null)
                        {
                            //AuditLogs.Add(log);
                            AuditLog.InsertOne(log);
                        }
                    }
                }
            }
            catch { }
        }
        #endregion

        #region MyAuditTrailHelperRegion
        private void SetModifiedProperties(EntityEntry entry, out string oldData, out string newData)
        {
            string json = string.Empty;
            List<AuditEntityProperties> auditEntitiesmodel = new List<AuditEntityProperties>();
            try
            {
                PropertyValues dbValues = entry.GetDatabaseValues();
                foreach (var propertyName in entry.OriginalValues.Properties)
                {
                    var oldVal = dbValues[propertyName.Name];
                    //to get dbValue
                    if (oldVal != null)
                    {
                        if (json.Length > 0)
                        {
                            json += ", ";
                        }
                        json += $@"""{propertyName}"":{(oldVal == null ? "null" : (IsNumber(oldVal) ? oldVal.ToString() : $@"""{oldVal.ToString().Replace("\"", "'")}"""))}";
                    }
                    //to get modified values
                    if (propertyName.Name != "RowVersion")
                    {
                        var newVal = entry.CurrentValues[propertyName];
                        if (oldVal != null && newVal != null && !Equals(oldVal, newVal))
                        {
                            AuditEntityProperties singleauditEntitymodel = new AuditEntityProperties();
                            singleauditEntitymodel.PropertyName = propertyName.Name;
                            singleauditEntitymodel.OldValue = oldVal.ToString();
                            singleauditEntitymodel.NewValue = newVal.ToString();
                            auditEntitiesmodel.Add(singleauditEntitymodel);
                        }
                    }

                }
                oldData = $"{{ {json} }}";
                newData = JsonConvert.SerializeObject(auditEntitiesmodel, Formatting.Indented,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
            }
            catch
            {
                oldData = $"{{ {json} }}";
                newData = string.Empty;
            }
        }
        private static string GetAsJson(PropertyValues values)
        {
            string json = string.Empty;
            try
            {
                if (values != null)
                {
                    foreach (var propertyName in values.Properties)
                    {

                        var val = values[propertyName.Name];
                        if (val != null)
                        {
                            if (json.Length > 0)
                            {
                                json += ", ";
                            }
                            json += $@"""{propertyName}"":{(val == null ? "null" : (IsNumber(val) ? val.ToString() : $@"""{val.ToString().Replace("\"", "'")}"""))}";
                        }

                    }
                }
            }
            catch { }
            return $"{{ {json} }}";
        }
        public static bool IsNumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }
        #endregion

        #region MyAuditHelperRegion
        private List<AuditLog> GetAuditRecordsForChange(EntityEntry dbEntry, long userId)
        {
            List<AuditLog> result = new List<AuditLog>();
            string IPAddress = string.Empty;
            try
            {
                try
                {
                    IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
                    IPAddress = heserver.AddressList.ToList().Where(p => p.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault().ToString();
                }
                catch
                {
                    IPAddress = "1::";
                }
                TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
                string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;

                if (tableName.ToUpper().Trim() != "ActivityLog".ToUpper().Trim())
                {
                    string jsonstring = string.Empty;
                    try
                    {
                        jsonstring = JsonConvert.SerializeObject(dbEntry.Entity, Formatting.Indented,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    }
                    catch { }
                    AuditLog auditlogmodel = new AuditLog();
                    auditlogmodel.UserId = userId;
                    auditlogmodel.EventDate = DateTime.Now;
                    auditlogmodel.TableName = tableName;
                    auditlogmodel.ColumnName = string.Empty;
                    auditlogmodel.IPAddress = IPAddress;
                    if (dbEntry.State == EntityState.Added)
                    {
                        auditlogmodel.EventType = Convert.ToInt32(AuditActionType.Create);
                        auditlogmodel.OldValue = "{  }";
                        auditlogmodel.NewValue = jsonstring;
                    }
                    else if (dbEntry.State == EntityState.Deleted)
                    {
                        auditlogmodel.EventType = Convert.ToInt32(AuditActionType.Delete);
                        auditlogmodel.OldValue = "{  }";
                        auditlogmodel.NewValue = jsonstring;
                    }
                    else if (dbEntry.State == EntityState.Modified)
                    {
                        var oldValues = string.Empty;
                        var modifiedValues = string.Empty;
                        SetModifiedProperties(dbEntry, out oldValues, out modifiedValues);
                        auditlogmodel.EventType = Convert.ToInt32(AuditActionType.Edit);
                        auditlogmodel.OldValue = oldValues;
                        auditlogmodel.NewValue = jsonstring;
                        auditlogmodel.ModifiedValue = modifiedValues;
                    }
                    result.Add(auditlogmodel);
                }
                return result;

            }
            catch
            {
                return result;
            }
        }
        #endregion

        private void SeedRoles(ModelBuilder builder)
        {
            Role[] roles = new Role[] {
                new Role { Id = 1, RoleName = "VGG_Admin", RoleType = "vgg_admin", CreateById = "seed"},
                new Role { Id = 2, RoleName = "CompanyAdmin", RoleType = "clientadmin", CreateById = "seed"},
                new Role { Id = 3, RoleName = "Interviewer", RoleType = "clientuser", CreateById = "seed"},
                new Role { Id = 4, RoleName = "Recruiter", RoleType = "clientuser", CreateById = "seed"}
            };
            builder.Entity<Role>().HasData(roles);
        }
    }
}
