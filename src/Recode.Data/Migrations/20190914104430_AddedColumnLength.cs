using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Recode.Data.Migrations
{
    public partial class AddedColumnLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "JobRoles",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Departments",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "JobRoles",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Departments",
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
                value: new DateTimeOffset(new DateTime(2019, 9, 13, 13, 33, 38, 723, DateTimeKind.Unspecified).AddTicks(8714), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2019, 9, 13, 13, 33, 38, 724, DateTimeKind.Unspecified).AddTicks(4324), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2019, 9, 13, 13, 33, 38, 724, DateTimeKind.Unspecified).AddTicks(4424), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DateCreated",
                value: new DateTimeOffset(new DateTime(2019, 9, 13, 13, 33, 38, 724, DateTimeKind.Unspecified).AddTicks(4431), new TimeSpan(0, 1, 0, 0, 0)));
        }
    }
}
