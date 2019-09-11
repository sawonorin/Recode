using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Recode.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comapanies",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateById = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comapanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateById = table.Column<string>(nullable: false),
                    CompanyId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateById = table.Column<string>(nullable: false),
                    Sender = table.Column<string>(maxLength: 1000, nullable: false),
                    Receiver = table.Column<string>(maxLength: 1000, nullable: false),
                    CC = table.Column<string>(maxLength: 1000, nullable: true),
                    BCC = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: false),
                    MessageBody = table.Column<string>(nullable: false),
                    Retires = table.Column<int>(nullable: false),
                    IsSent = table.Column<bool>(nullable: false),
                    DateSent = table.Column<DateTimeOffset>(nullable: true),
                    DateToSend = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateById = table.Column<string>(nullable: false),
                    DepartmentId = table.Column<long>(nullable: true),
                    CompanyId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateById = table.Column<string>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 10, nullable: false),
                    RoleType = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateById = table.Column<string>(nullable: false),
                    SSOUserId = table.Column<string>(nullable: false),
                    CompanyId = table.Column<long>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateById = table.Column<string>(nullable: false),
                    CompanyId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateById = table.Column<string>(nullable: false),
                    DepartmentId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobRoles_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateById = table.Column<string>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateById = table.Column<string>(nullable: false),
                    CompanyId = table.Column<long>(nullable: false),
                    JobRoleId = table.Column<long>(nullable: false),
                    DepartmentId = table.Column<long>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    InterviewStage = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidates_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidates_JobRoles_JobRoleId",
                        column: x => x.JobRoleId,
                        principalTable: "JobRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewSessions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateById = table.Column<string>(nullable: false),
                    CompanyId = table.Column<long>(nullable: false),
                    DepartmentId = table.Column<long>(nullable: false),
                    JobRoleId = table.Column<long>(nullable: false),
                    RecruiterId = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    VenueId = table.Column<long>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewSessions_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewSessions_JobRoles_JobRoleId",
                        column: x => x.JobRoleId,
                        principalTable: "JobRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewSessions_Users_RecruiterId",
                        column: x => x.RecruiterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewSessions_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewSessionCandidates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateById = table.Column<string>(nullable: false),
                    CandidateId = table.Column<long>(nullable: false),
                    InterviewSessionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewSessionCandidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewSessionCandidates_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewSessionCandidates_InterviewSessions_InterviewSessionId",
                        column: x => x.InterviewSessionId,
                        principalTable: "InterviewSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewSessionInterviewers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateById = table.Column<string>(nullable: false),
                    InterviewerId = table.Column<long>(nullable: false),
                    InterviewSessionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewSessionInterviewers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewSessionInterviewers_InterviewSessions_InterviewSessionId",
                        column: x => x.InterviewSessionId,
                        principalTable: "InterviewSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewSessionInterviewers_Users_InterviewerId",
                        column: x => x.InterviewerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewSessionMetrics",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateById = table.Column<string>(nullable: false),
                    MetricId = table.Column<long>(nullable: false),
                    InterviewSessionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewSessionMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewSessionMetrics_InterviewSessions_InterviewSessionId",
                        column: x => x.InterviewSessionId,
                        principalTable: "InterviewSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewSessionMetrics_Metrics_MetricId",
                        column: x => x.MetricId,
                        principalTable: "Metrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewSessionResults",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateById = table.Column<string>(nullable: false),
                    CandidateId = table.Column<long>(nullable: false),
                    InterviewSessionId = table.Column<long>(nullable: false),
                    MetricId = table.Column<long>(nullable: false),
                    Rating = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewSessionResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewSessionResults_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewSessionResults_InterviewSessions_InterviewSessionId",
                        column: x => x.InterviewSessionId,
                        principalTable: "InterviewSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewSessionResults_Metrics_MetricId",
                        column: x => x.MetricId,
                        principalTable: "Metrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateById", "DateCreated", "Description", "IsActive", "IsDeleted", "RoleName", "RoleType" },
                values: new object[,]
                {
                    { 1L, "seed", new DateTimeOffset(new DateTime(2019, 9, 11, 7, 29, 51, 319, DateTimeKind.Unspecified).AddTicks(8813), new TimeSpan(0, 1, 0, 0, 0)), null, true, false, "VGG_Admin", "vgg_admin" },
                    { 2L, "seed", new DateTimeOffset(new DateTime(2019, 9, 11, 7, 29, 51, 320, DateTimeKind.Unspecified).AddTicks(1085), new TimeSpan(0, 1, 0, 0, 0)), null, true, false, "CompanyAdmin", "clientadmin" },
                    { 3L, "seed", new DateTimeOffset(new DateTime(2019, 9, 11, 7, 29, 51, 320, DateTimeKind.Unspecified).AddTicks(1101), new TimeSpan(0, 1, 0, 0, 0)), null, true, false, "Interviewer", "clientuser" },
                    { 4L, "seed", new DateTimeOffset(new DateTime(2019, 9, 11, 7, 29, 51, 320, DateTimeKind.Unspecified).AddTicks(1103), new TimeSpan(0, 1, 0, 0, 0)), null, true, false, "Recruiter", "clientuser" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_CompanyId",
                table: "Candidates",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_DepartmentId",
                table: "Candidates",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_JobRoleId",
                table: "Candidates",
                column: "JobRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CompanyId",
                table: "Departments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessionCandidates_CandidateId",
                table: "InterviewSessionCandidates",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessionCandidates_InterviewSessionId",
                table: "InterviewSessionCandidates",
                column: "InterviewSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessionInterviewers_InterviewSessionId",
                table: "InterviewSessionInterviewers",
                column: "InterviewSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessionInterviewers_InterviewerId",
                table: "InterviewSessionInterviewers",
                column: "InterviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessionMetrics_InterviewSessionId",
                table: "InterviewSessionMetrics",
                column: "InterviewSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessionMetrics_MetricId",
                table: "InterviewSessionMetrics",
                column: "MetricId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessionResults_CandidateId",
                table: "InterviewSessionResults",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessionResults_InterviewSessionId",
                table: "InterviewSessionResults",
                column: "InterviewSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessionResults_MetricId",
                table: "InterviewSessionResults",
                column: "MetricId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessions_CompanyId",
                table: "InterviewSessions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessions_DepartmentId",
                table: "InterviewSessions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessions_JobRoleId",
                table: "InterviewSessions",
                column: "JobRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessions_RecruiterId",
                table: "InterviewSessions",
                column: "RecruiterId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessions_VenueId",
                table: "InterviewSessions",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRoles_DepartmentId",
                table: "JobRoles",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_CompanyId",
                table: "Metrics",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SSOUserId",
                table: "Users",
                column: "SSOUserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comapanies");

            migrationBuilder.DropTable(
                name: "EmailLogs");

            migrationBuilder.DropTable(
                name: "InterviewSessionCandidates");

            migrationBuilder.DropTable(
                name: "InterviewSessionInterviewers");

            migrationBuilder.DropTable(
                name: "InterviewSessionMetrics");

            migrationBuilder.DropTable(
                name: "InterviewSessionResults");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "InterviewSessions");

            migrationBuilder.DropTable(
                name: "Metrics");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "JobRoles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Venues");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
