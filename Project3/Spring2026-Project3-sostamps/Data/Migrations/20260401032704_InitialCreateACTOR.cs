using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spring2026_Project3_sostamps.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateACTOR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Actors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IMDBLink",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Portrait",
                table: "Actors",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "IMDBLink",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "Portrait",
                table: "Actors");
        }
    }
}
