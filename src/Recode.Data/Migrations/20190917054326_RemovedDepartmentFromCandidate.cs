using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Recode.Data.Migrations
{
    public partial class RemovedDepartmentFromCandidate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_Departments_DepartmentId",
                table: "Candidates");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_JobRoles_JobRoleId",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_DepartmentId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Candidates");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Candidates",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "JobRoleId",
                table: "Candidates",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumeUrl",
                table: "Candidates",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2019, 9, 17, 6, 43, 26, 495, DateTimeKind.Unspecified).AddTicks(7038), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2019, 9, 17, 6, 43, 26, 495, DateTimeKind.Unspecified).AddTicks(9235), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2019, 9, 17, 6, 43, 26, 495, DateTimeKind.Unspecified).AddTicks(9251), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2019, 9, 17, 6, 43, 26, 495, DateTimeKind.Unspecified).AddTicks(9253), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_JobRoles_JobRoleId",
                table: "Candidates",
                column: "JobRoleId",
                principalTable: "JobRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_JobRoles_JobRoleId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "ResumeUrl",
                table: "Candidates");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Candidates",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "JobRoleId",
                table: "Candidates",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "Candidates",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2019, 9, 16, 21, 2, 37, 973, DateTimeKind.Unspecified).AddTicks(2378), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2019, 9, 16, 21, 2, 37, 973, DateTimeKind.Unspecified).AddTicks(4574), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2019, 9, 16, 21, 2, 37, 973, DateTimeKind.Unspecified).AddTicks(4591), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2019, 9, 16, 21, 2, 37, 973, DateTimeKind.Unspecified).AddTicks(4593), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_DepartmentId",
                table: "Candidates",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_Departments_DepartmentId",
                table: "Candidates",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_JobRoles_JobRoleId",
                table: "Candidates",
                column: "JobRoleId",
                principalTable: "JobRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
