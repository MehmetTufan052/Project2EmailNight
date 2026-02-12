using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project2EmailNight.Migrations
{
    public partial class add_email_category_fk_nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmailCategoryId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmailCategories",
                columns: table => new
                {
                    EmailCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailCategories", x => x.EmailCategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_EmailCategoryId",
                table: "Messages",
                column: "EmailCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_EmailCategories_EmailCategoryId",
                table: "Messages",
                column: "EmailCategoryId",
                principalTable: "EmailCategories",
                principalColumn: "EmailCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_EmailCategories_EmailCategoryId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "EmailCategories");

            migrationBuilder.DropIndex(
                name: "IX_Messages_EmailCategoryId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "EmailCategoryId",
                table: "Messages");
        }
    }
}
