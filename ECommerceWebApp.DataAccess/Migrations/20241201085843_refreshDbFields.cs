using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceWebApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class refreshDbFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUri",
                table: "Products",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Products",
                newName: "ImageUri");
        }
    }
}
