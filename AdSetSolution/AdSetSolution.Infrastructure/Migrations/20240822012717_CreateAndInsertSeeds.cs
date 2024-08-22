using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdSetSolution.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateAndInsertSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Optional",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Optionals", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Optional",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Ar Condicionado" },
                    { 2, "Alarme" },
                    { 3, "Airbag" },
                    { 4, "Freio ABS" },
                    { 5, "Direção Hidráulica" },
                    { 6, "Roda de Liga Leve" },
                    { 7, "Vidro Elétrico" },
                    { 8, "Sensor de Estacionamento" },
                    { 9, "Câmera de Ré" },
                    { 10, "Controle de Cruzeiro" }
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false),
                    PortalType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.InsertData(
               table: "Packages",
               columns: new[] { "Id", "Name", "Total", "PortalType" },
               values: new object[,]
               {
                    { 1, "Diamante Feirão", 18, 1 },
                    { 2, "Diamante", 55, 1 },
                    { 3, "Platinum", 50, 1 },
                    { 4, "Básico", 55, 2 }
               });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleImgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleImgs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleImgs_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleOptional",
                columns: table => new
                {
                    OptionalId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleOptional", x => new { x.VehicleId, x.OptionalId });
                    table.ForeignKey(
                        name: "FK_VehicleOptional_Optionals_OptionalId",
                        column: x => x.OptionalId,
                        principalTable: "Optionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleOptional_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehiclePackages",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    PortalType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiclePackages", x => new { x.VehicleId, x.PackageId, x.PortalType });
                    table.ForeignKey(
                        name: "FK_VehiclePackages_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehiclePackages_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleImgs_VehicleId",
                table: "VehicleImgs",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleOptional_OptionalId",
                table: "VehicleOptional",
                column: "OptionalId");

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePackages_PackageId",
                table: "VehiclePackages",
                column: "PackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleImgs");

            migrationBuilder.DropTable(
                name: "VehicleOptional");

            migrationBuilder.DropTable(
                name: "VehiclePackages");

            migrationBuilder.DropTable(
                name: "Optionals");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
