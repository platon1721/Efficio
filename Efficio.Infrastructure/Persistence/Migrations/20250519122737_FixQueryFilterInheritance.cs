using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Efficio.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixQueryFilterInheritance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId1",
                table: "UserDepartments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDepartments_DepartmentId1",
                table: "UserDepartments",
                column: "DepartmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartments_Departments_DepartmentId1",
                table: "UserDepartments",
                column: "DepartmentId1",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartments_Departments_DepartmentId1",
                table: "UserDepartments");

            migrationBuilder.DropIndex(
                name: "IX_UserDepartments_DepartmentId1",
                table: "UserDepartments");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "UserDepartments");
        }
    }
}
