using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddVerlatenColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SpelerVerlaten",
                table: "Spellen",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpelerVerlaten",
                table: "Spellen");
        }
    }
}
