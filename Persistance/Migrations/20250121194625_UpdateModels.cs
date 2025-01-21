using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Mail",
                table: "AspNetUsers",
                newName: "RoleName");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id",
                column: "CreationDate",
                value: new DateTime(2025, 1, 21, 19, 46, 24, 754, DateTimeKind.Utc).AddTicks(5947));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "Address", "ConcurrencyStamp", "LastModifiedDate", "PasswordHash" },
                values: new object[] { null, "575726f8-d40b-456b-9020-e988aaca0804", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAELfOvMT4IC3AK9doxvA7Yx0bZrzujOxPmhQWeMFug1OoXr96x03BIAs5t6zkdbakxA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RoleName",
                table: "AspNetUsers",
                newName: "Mail");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id",
                column: "CreationDate",
                value: new DateTime(2025, 1, 18, 21, 45, 24, 332, DateTimeKind.Utc).AddTicks(8458));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ddddea86-2a3d-49f4-8531-15a826ee4550", "AQAAAAIAAYagAAAAED4LIrFx98Vk/KEbLb9YJq0EscBrKauUF4hCPZkOUVfH0h192O0x71oMordUA/ImfA==" });
        }
    }
}
