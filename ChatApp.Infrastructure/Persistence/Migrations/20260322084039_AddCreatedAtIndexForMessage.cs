using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtIndexForMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateIndex(
                name: "Messages_CreatedAt_idx",
                table: "Messages",
                column: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropIndex(
                name: "Messages_CreatedAt_idx",
                table: "Messages");
        }
    }
}
