using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDefaultsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "user-role-id", "35910f40-5f2f-44d4-83a7-c77a279375dc" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "manager-role-id", "81eb8672-a56f-4a06-9f3d-a97d387bda7e" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "admin-role-id", "8b5ba644-bdfb-4e80-920d-1d4177f64857" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "35910f40-5f2f-44d4-83a7-c77a279375dc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "81eb8672-a56f-4a06-9f3d-a97d387bda7e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8b5ba644-bdfb-4e80-920d-1d4177f64857");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id",
                column: "CreationDate",
                value: new DateTime(2025, 1, 24, 18, 9, 1, 902, DateTimeKind.Utc).AddTicks(5911));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "manager-role-id",
                column: "CreationDate",
                value: new DateTime(2025, 1, 24, 18, 9, 1, 902, DateTimeKind.Utc).AddTicks(5918));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-role-id",
                column: "CreationDate",
                value: new DateTime(2025, 1, 24, 18, 9, 1, 902, DateTimeKind.Utc).AddTicks(5915));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DateOfCreation", "Email", "EmailConfirmed", "FirstName", "LastModifiedDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RoleName", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "0304213c-d14f-41a9-ab04-0fe7c35cd3cf", 0, null, "9b0535bb-0487-4545-b624-8b75b5bc9bc2", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "manager@example.com", true, "Manager", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", false, null, "MANAGER@EXAMPLE.COM", "MANAGER", "AQAAAAIAAYagAAAAEMS+vedx3USTIAv/5cEuCIqpwbWpg3+JUgob03GV/rFtvP24z+uR93UDUQCWRrhmPw==", null, false, "Manager", "", false, "manager" },
                    { "48597bad-e225-4665-ab56-4a124c5997f1", 0, null, "158128bc-b798-4640-bff3-48cdbb4b821d", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user@example.com", true, "User", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", false, null, "USER@EXAMPLE.COM", "USER", "AQAAAAIAAYagAAAAEOCEe0XftqlG53ilBdqY9+h3taJXv29yj887Ye7W6oi0rPeS3UNBSs4UmllwUpY91Q==", null, false, "User", "", false, "user" },
                    { "5ceb01cf-723b-4713-8294-7631fd246703", 0, null, "ca45eb76-02a8-49e6-82ff-69281888d7d9", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", true, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEPCCi6vRnkqKIr4ZFfDzMZHs3QwrZGPQgr4LjOsBtO5so8Rv7uqExUdzat1UkrWh5g==", null, false, "Admin", "", false, "admin" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { "04271915-cf81-435e-8cf5-f0e26b202ac3", "Description for Android Phone", "Android Phone", 250m },
                    { "173a2027-75c4-4b88-ba7d-9eebc5a45eee", "Description for Notebook", "Notebook", 8500m },
                    { "2bf8d00e-dde9-4208-9420-2da2b58dffd7", "Description for IPhone", "IPhone", 350m },
                    { "552c4976-e0bb-4080-86f9-1e701257e990", "Description for HP PC", "HP PC", 1500m },
                    { "6066e4cf-1029-4370-93df-bb215cccba40", "Description for Phone", "Phone", 150m },
                    { "6b600f3a-2a98-4135-b4d8-97a38c7acdc2", "Description for Android used", "Android used", 50m },
                    { "7fbbe7bc-c846-4311-91f0-982c16127fd4", "Description for Head Phones", "Head Phones", 50m },
                    { "b9d1ba24-c574-45f9-b86f-3b12d8b43b6a", "Description for Monitor 27", "Monitor 27", 230m },
                    { "e8d7532d-bd26-479f-adb2-cea026da152a", "Description for Monitor 24", "Monitor 24", 175m },
                    { "f688443e-bd08-48cb-b508-e3cf4f4f506b", "Description for Mouse", "Mouse", 20m }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "manager-role-id", "0304213c-d14f-41a9-ab04-0fe7c35cd3cf" },
                    { "user-role-id", "48597bad-e225-4665-ab56-4a124c5997f1" },
                    { "admin-role-id", "5ceb01cf-723b-4713-8294-7631fd246703" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "manager-role-id", "0304213c-d14f-41a9-ab04-0fe7c35cd3cf" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "user-role-id", "48597bad-e225-4665-ab56-4a124c5997f1" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "admin-role-id", "5ceb01cf-723b-4713-8294-7631fd246703" });

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "04271915-cf81-435e-8cf5-f0e26b202ac3");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "173a2027-75c4-4b88-ba7d-9eebc5a45eee");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "2bf8d00e-dde9-4208-9420-2da2b58dffd7");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "552c4976-e0bb-4080-86f9-1e701257e990");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "6066e4cf-1029-4370-93df-bb215cccba40");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "6b600f3a-2a98-4135-b4d8-97a38c7acdc2");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "7fbbe7bc-c846-4311-91f0-982c16127fd4");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "b9d1ba24-c574-45f9-b86f-3b12d8b43b6a");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "e8d7532d-bd26-479f-adb2-cea026da152a");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "f688443e-bd08-48cb-b508-e3cf4f4f506b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0304213c-d14f-41a9-ab04-0fe7c35cd3cf");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "48597bad-e225-4665-ab56-4a124c5997f1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5ceb01cf-723b-4713-8294-7631fd246703");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id",
                column: "CreationDate",
                value: new DateTime(2025, 1, 24, 18, 1, 44, 905, DateTimeKind.Utc).AddTicks(24));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "manager-role-id",
                column: "CreationDate",
                value: new DateTime(2025, 1, 24, 18, 1, 44, 905, DateTimeKind.Utc).AddTicks(31));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-role-id",
                column: "CreationDate",
                value: new DateTime(2025, 1, 24, 18, 1, 44, 905, DateTimeKind.Utc).AddTicks(29));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DateOfCreation", "Email", "EmailConfirmed", "FirstName", "LastModifiedDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RoleName", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "35910f40-5f2f-44d4-83a7-c77a279375dc", 0, null, "fa5a2f0a-d9ba-4d44-9080-d4e63140bd7e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user@example.com", true, "User", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", false, null, "USER@EXAMPLE.COM", "USER", "AQAAAAIAAYagAAAAEPC/oRjSe9AtWQ8TWwp2yRjWqzcElfCPX7dE1tb9zbAaP6zCFrQrZTOXM0J4wfXsZQ==", null, false, "User", "", false, "user" },
                    { "81eb8672-a56f-4a06-9f3d-a97d387bda7e", 0, null, "299d9225-9973-40ae-aa13-a63daaf55842", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "manager@example.com", true, "Manager", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", false, null, "MANAGER@EXAMPLE.COM", "MANAGER", "AQAAAAIAAYagAAAAEEH+IqNcCCM/PAjtyMGbIwLsSQDjZAJ1RtxKxSH/cnv/ekHGL2hETh+WypQJOa5dJw==", null, false, "Manager", "", false, "manager" },
                    { "8b5ba644-bdfb-4e80-920d-1d4177f64857", 0, null, "845bf2b7-2765-43a1-b397-c6b773e1ad5c", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", true, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEMhE+S0CgeCEb700lCxqGRt0KmH5owaaXxlu4ZjvGiQRPkpsWhIRTH8jkN+p6t6xMw==", null, false, "Admin", "", false, "admin" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "user-role-id", "35910f40-5f2f-44d4-83a7-c77a279375dc" },
                    { "manager-role-id", "81eb8672-a56f-4a06-9f3d-a97d387bda7e" },
                    { "admin-role-id", "8b5ba644-bdfb-4e80-920d-1d4177f64857" }
                });
        }
    }
}
