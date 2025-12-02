using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameAccountsToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint first
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Accounts_AccountId",
                table: "Customers");

            // Rename the Accounts table to Users
            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Users");

            // Rename the AccountId column to UserId in Customers table
            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Customers",
                newName: "UserId");

            // Rename indexes on Users table
            migrationBuilder.RenameIndex(
                name: "IX_Accounts_Email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_EmailVerificationToken",
                table: "Users",
                newName: "IX_Users_EmailVerificationToken");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_PasswordResetToken",
                table: "Users",
                newName: "IX_Users_PasswordResetToken");

            // Rename index on Customers table
            migrationBuilder.RenameIndex(
                name: "IX_Customers_AccountId",
                table: "Customers",
                newName: "IX_Customers_UserId");

            // Rename primary key constraint
            migrationBuilder.Sql("ALTER TABLE \"Users\" RENAME CONSTRAINT \"PK_Accounts\" TO \"PK_Users\"");

            // Re-add foreign key with new name
            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_UserId",
                table: "Customers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint first
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_UserId",
                table: "Customers");

            // Rename the Users table back to Accounts
            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Accounts");

            // Rename the UserId column back to AccountId in Customers table
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Customers",
                newName: "AccountId");

            // Rename indexes on Accounts table
            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "Accounts",
                newName: "IX_Accounts_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Users_EmailVerificationToken",
                table: "Accounts",
                newName: "IX_Accounts_EmailVerificationToken");

            migrationBuilder.RenameIndex(
                name: "IX_Users_PasswordResetToken",
                table: "Accounts",
                newName: "IX_Accounts_PasswordResetToken");

            // Rename index on Customers table
            migrationBuilder.RenameIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                newName: "IX_Customers_AccountId");

            // Rename primary key constraint
            migrationBuilder.Sql("ALTER TABLE \"Accounts\" RENAME CONSTRAINT \"PK_Users\" TO \"PK_Accounts\"");

            // Re-add foreign key with old name
            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Accounts_AccountId",
                table: "Customers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
