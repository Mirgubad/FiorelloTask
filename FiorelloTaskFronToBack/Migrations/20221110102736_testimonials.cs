using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiorelloTaskFronToBack.Migrations
{
    public partial class testimonials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BlogTexts_BlogId",
                table: "BlogTexts");

            migrationBuilder.CreateTable(
                name: "Testimonials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    FlowerExpertId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Testimonials_FlowerExperts_FlowerExpertId",
                        column: x => x.FlowerExpertId,
                        principalTable: "FlowerExperts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogTexts_BlogId",
                table: "BlogTexts",
                column: "BlogId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_FlowerExpertId",
                table: "Testimonials",
                column: "FlowerExpertId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Testimonials");

            migrationBuilder.DropIndex(
                name: "IX_BlogTexts_BlogId",
                table: "BlogTexts");

            migrationBuilder.CreateIndex(
                name: "IX_BlogTexts_BlogId",
                table: "BlogTexts",
                column: "BlogId");
        }
    }
}
