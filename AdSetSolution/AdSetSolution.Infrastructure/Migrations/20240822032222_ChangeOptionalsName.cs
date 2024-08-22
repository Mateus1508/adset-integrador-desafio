using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdSetSolution.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOptionalsName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleOptional_Optionals_OptionalId",
                table: "VehicleOptional");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Optionals",
                table: "Optionals");

            migrationBuilder.RenameTable(
                name: "Optionals",
                newName: "Optional");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Optional",
                table: "Optional",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleOptional_Optional_OptionalId",
                table: "VehicleOptional",
                column: "OptionalId",
                principalTable: "Optional",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleOptional_Optional_OptionalId",
                table: "VehicleOptional");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Optional",
                table: "Optional");

            migrationBuilder.RenameTable(
                name: "Optional",
                newName: "Optionals");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Optionals",
                table: "Optionals",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleOptional_Optionals_OptionalId",
                table: "VehicleOptional",
                column: "OptionalId",
                principalTable: "Optionals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
