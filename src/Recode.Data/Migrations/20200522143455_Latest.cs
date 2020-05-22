using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Recode.Data.Migrations
{
    public partial class Latest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "InterviewSessions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "InterviewSessions",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2020, 5, 22, 15, 34, 55, 480, DateTimeKind.Unspecified).AddTicks(8284), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2020, 5, 22, 15, 34, 55, 481, DateTimeKind.Unspecified).AddTicks(817), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2020, 5, 22, 15, 34, 55, 481, DateTimeKind.Unspecified).AddTicks(833), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2020, 5, 22, 15, 34, 55, 481, DateTimeKind.Unspecified).AddTicks(836), new TimeSpan(0, 1, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "InterviewSessions");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "InterviewSessions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

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
        }
    }
}
