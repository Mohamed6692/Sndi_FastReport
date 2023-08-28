using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActeAdministratif.Migrations
{
    public partial class version7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "DemandeInit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "T_CONF_COUNTRY",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AbrevTwo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    AbrevThree = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Name_en = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Name_fr = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CONF_COUNTRY", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_CONF_COUNTRY");

            migrationBuilder.DropColumn(
                name: "status",
                table: "DemandeInit");
        }
    }
}
