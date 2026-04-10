using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDisplayNameToUserConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Conversations");

            _ = migrationBuilder.AddColumn<string>(
                name: "UserDisplayName",
                table: "UserConversations",
                type: "text",
                nullable: false,
                defaultValue: "");

            _ = migrationBuilder.AddColumn<DateTime>(
                name: "LastMessageAt",
                table: "Conversations",
                type: "timestamp with time zone",
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "LastMessageContent",
                table: "Conversations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "UserDisplayName",
                table: "UserConversations");

            _ = migrationBuilder.DropColumn(
                name: "LastMessageAt",
                table: "Conversations");

            _ = migrationBuilder.DropColumn(
                name: "LastMessageContent",
                table: "Conversations");

            _ = migrationBuilder.AddColumn<long>(
                name: "RoomId",
                table: "Conversations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
