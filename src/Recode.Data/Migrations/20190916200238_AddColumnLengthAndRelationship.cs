using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Recode.Data.Migrations
{
    public partial class AddColumnLengthAndRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Venues",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Metrics",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

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
                name: "IX_Metrics_DepartmentId",
                table: "Metrics",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Metrics_Departments_DepartmentId",
                table: "Metrics",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Metrics_Departments_DepartmentId",
                table: "Metrics");

            migrationBuilder.DropIndex(
                name: "IX_Metrics_DepartmentId",
                table: "Metrics");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Venues",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Metrics",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2019, 9, 14, 11, 44, 29, 868, DateTimeKind.Unspecified).AddTicks(7667), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2019, 9, 14, 11, 44, 29, 868, DateTimeKind.Unspecified).AddTicks(9962), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2019, 9, 14, 11, 44, 29, 868, DateTimeKind.Unspecified).AddTicks(9980), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2019, 9, 14, 11, 44, 29, 868, DateTimeKind.Unspecified).AddTicks(9982), new TimeSpan(0, 1, 0, 0, 0)));
        }
    }
}
