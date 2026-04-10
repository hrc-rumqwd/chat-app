using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditableForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Users",
                type: "text",
                nullable: true);

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsActived",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Users",
                type: "text",
                nullable: true);

            _ = migrationBuilder.AddColumn<long>(
                name: "GroupId",
                table: "Messages",
                type: "bigint",
                nullable: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Messages_GroupId",
                table: "Messages",
                column: "GroupId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Messages_Groups_GroupId",
                table: "Messages",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Messages_Groups_GroupId",
                table: "Messages");

            _ = migrationBuilder.DropIndex(
                name: "IX_Messages_GroupId",
                table: "Messages");

            _ = migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            _ = migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Users");

            _ = migrationBuilder.DropColumn(
                name: "IsActived",
                table: "Users");

            _ = migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            _ = migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Users");

            _ = migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Users");

            _ = migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Messages");
        }
    }
}
