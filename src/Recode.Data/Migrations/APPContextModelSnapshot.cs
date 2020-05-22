﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Recode.Data;

namespace Recode.Data.Migrations
{
    [DbContext(typeof(APPContext))]
    partial class APPContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Recode.Data.AppEntity.Candidate", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CompanyId");

                    b.Property<string>("CreateById")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<string>("Email")
                        .HasMaxLength(100);

                    b.Property<string>("FirstName")
                        .HasMaxLength(50);

                    b.Property<int>("InterviewStage");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<long>("JobRoleId");

                    b.Property<string>("LastName")
                        .HasMaxLength(50);

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(50);

                    b.Property<string>("ResumeUrl");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("JobRoleId");

                    b.ToTable("Candidates");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.Company", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<string>("CreateById")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.Department", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CompanyId");

                    b.Property<string>("CreateById")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.EmailLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BCC");

                    b.Property<string>("CC")
                        .HasMaxLength(1000);

                    b.Property<string>("CreateById")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<DateTimeOffset?>("DateSent");

                    b.Property<DateTimeOffset>("DateToSend");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsSent");

                    b.Property<string>("MessageBody")
                        .IsRequired();

                    b.Property<string>("Receiver")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int>("Retires");

                    b.Property<string>("Sender")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("EmailLogs");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.InterviewSession", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CompanyId");

                    b.Property<string>("CreateById")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<DateTime>("EndTime");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<long>("JobRoleId");

                    b.Property<long>("RecruiterId");

                    b.Property<DateTime>("StartTime");

                    b.Property<string>("Status");

                    b.Property<string>("Subject");

                    b.Property<long?>("VenueId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("JobRoleId");

                    b.HasIndex("RecruiterId");

                    b.HasIndex("VenueId");

                    b.ToTable("InterviewSessions");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.InterviewSessionCandidate", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CandidateId");

                    b.Property<string>("CreateById")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<long>("InterviewSessionId");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.HasKey("Id");

                    b.HasIndex("CandidateId");

                    b.HasIndex("InterviewSessionId");

                    b.ToTable("InterviewSessionCandidates");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.InterviewSessionInterviewer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreateById")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<long>("InterviewSessionId");

                    b.Property<long>("InterviewerId");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.HasKey("Id");

                    b.HasIndex("InterviewSessionId");

                    b.HasIndex("InterviewerId");

                    b.ToTable("InterviewSessionInterviewers");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.InterviewSessionMetric", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreateById")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<long>("InterviewSessionId");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<long>("MetricId");

                    b.HasKey("Id");

                    b.HasIndex("InterviewSessionId");

                    b.HasIndex("MetricId");

                    b.ToTable("InterviewSessionMetrics");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.InterviewSessionResult", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CandidateId");

                    b.Property<string>("CreateById")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<long>("InterviewSessionMetricId");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("Rating");

                    b.Property<string>("Remark")
                        .HasMaxLength(400);

                    b.HasKey("Id");

                    b.HasIndex("CandidateId");

                    b.HasIndex("InterviewSessionMetricId");

                    b.ToTable("InterviewSessionResults");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.JobRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreateById")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<long>("DepartmentId");

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("JobRoles");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.Metric", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CompanyId");

                    b.Property<string>("CreateById")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<long?>("DepartmentId");

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Metrics");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreateById")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<string>("Description");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("RoleType")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreateById = "seed",
                            DateCreated = new DateTimeOffset(new DateTime(2020, 5, 22, 15, 34, 55, 480, DateTimeKind.Unspecified).AddTicks(8284), new TimeSpan(0, 1, 0, 0, 0)),
                            IsActive = true,
                            IsDeleted = false,
                            RoleName = "VGG_Admin",
                            RoleType = "vgg_admin"
                        },
                        new
                        {
                            Id = 2L,
                            CreateById = "seed",
                            DateCreated = new DateTimeOffset(new DateTime(2020, 5, 22, 15, 34, 55, 481, DateTimeKind.Unspecified).AddTicks(817), new TimeSpan(0, 1, 0, 0, 0)),
                            IsActive = true,
                            IsDeleted = false,
                            RoleName = "CompanyAdmin",
                            RoleType = "clientadmin"
                        },
                        new
                        {
                            Id = 3L,
                            CreateById = "seed",
                            DateCreated = new DateTimeOffset(new DateTime(2020, 5, 22, 15, 34, 55, 481, DateTimeKind.Unspecified).AddTicks(833), new TimeSpan(0, 1, 0, 0, 0)),
                            IsActive = true,
                            IsDeleted = false,
                            RoleName = "Interviewer",
                            RoleType = "clientuser"
                        },
                        new
                        {
                            Id = 4L,
                            CreateById = "seed",
                            DateCreated = new DateTimeOffset(new DateTime(2020, 5, 22, 15, 34, 55, 481, DateTimeKind.Unspecified).AddTicks(836), new TimeSpan(0, 1, 0, 0, 0)),
                            IsActive = true,
                            IsDeleted = false,
                            RoleName = "Recruiter",
                            RoleType = "clientuser"
                        });
                });

            modelBuilder.Entity("Recode.Data.AppEntity.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CompanyId");

                    b.Property<string>("CreateById")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(50);

                    b.Property<string>("SSOUserId")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("SSOUserId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.UserRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreateById")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<long>("RoleId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.Venue", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CompanyId");

                    b.Property<string>("CreateById")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<string>("Description");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Venues");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.Candidate", b =>
                {
                    b.HasOne("Recode.Data.AppEntity.JobRole", "JobRole")
                        .WithMany("Candidates")
                        .HasForeignKey("JobRoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Recode.Data.AppEntity.InterviewSession", b =>
                {
                    b.HasOne("Recode.Data.AppEntity.JobRole", "JobRole")
                        .WithMany()
                        .HasForeignKey("JobRoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Recode.Data.AppEntity.User", "Recruiter")
                        .WithMany()
                        .HasForeignKey("RecruiterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Recode.Data.AppEntity.Venue", "Venue")
                        .WithMany()
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.InterviewSessionCandidate", b =>
                {
                    b.HasOne("Recode.Data.AppEntity.Candidate", "Candidate")
                        .WithMany()
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Recode.Data.AppEntity.InterviewSessionInterviewer", b =>
                {
                    b.HasOne("Recode.Data.AppEntity.User", "Interviewer")
                        .WithMany()
                        .HasForeignKey("InterviewerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Recode.Data.AppEntity.InterviewSessionMetric", b =>
                {
                    b.HasOne("Recode.Data.AppEntity.Metric", "Metric")
                        .WithMany()
                        .HasForeignKey("MetricId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Recode.Data.AppEntity.InterviewSessionResult", b =>
                {
                    b.HasOne("Recode.Data.AppEntity.Candidate", "Candidate")
                        .WithMany()
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Recode.Data.AppEntity.InterviewSessionMetric", "InterviewSessionMetric")
                        .WithMany()
                        .HasForeignKey("InterviewSessionMetricId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Recode.Data.AppEntity.JobRole", b =>
                {
                    b.HasOne("Recode.Data.AppEntity.Department", "Department")
                        .WithMany("JobRoles")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Recode.Data.AppEntity.Metric", b =>
                {
                    b.HasOne("Recode.Data.AppEntity.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");
                });

            modelBuilder.Entity("Recode.Data.AppEntity.UserRole", b =>
                {
                    b.HasOne("Recode.Data.AppEntity.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Recode.Data.AppEntity.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
