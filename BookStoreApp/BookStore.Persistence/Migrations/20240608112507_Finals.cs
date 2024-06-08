using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Finals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookIDs",
                table: "Carts",
                newName: "SessionId");

            migrationBuilder.AddColumn<string>(
                name: "BookId",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "Carts",
                newName: "BookIDs");
        }
    }
}
