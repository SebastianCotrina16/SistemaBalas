using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace frontend_admin.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedExternalUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_usuarios_users_dni",
                table: "usuarios");

            migrationBuilder.DropIndex(
                name: "IX_usuarios_dni",
                table: "usuarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_usuarios_dni",
                table: "usuarios",
                column: "dni");

            migrationBuilder.AddForeignKey(
                name: "FK_usuarios_users_dni",
                table: "usuarios",
                column: "dni",
                principalTable: "users",
                principalColumn: "dni",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
