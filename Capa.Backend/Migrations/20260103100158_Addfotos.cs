using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capa.Backend.Migrations
{
    /// <inheritdoc />
    public partial class Addfotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Estudiantes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Estudiantes");
        }
    }
}
