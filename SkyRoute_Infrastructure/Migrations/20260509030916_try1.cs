using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkyRoute_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class try1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FlightDepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PriceTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsInternational = table.Column<bool>(type: "bit", nullable: false),
                    CabinType = table.Column<int>(type: "int", nullable: false),
                    JSONSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPassport = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passengers_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    CodeIATA = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PublicName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Airports_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeDeparture = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeArrival = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BaseFare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CabinType = table.Column<int>(type: "int", nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AirportOriginId = table.Column<int>(type: "int", nullable: false),
                    AirportDestinationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flights_Airports_AirportDestinationId",
                        column: x => x.AirportDestinationId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flights_Airports_AirportOriginId",
                        column: x => x.AirportOriginId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "United States" },
                    { 2, "United Kingdom" },
                    { 3, "Argentina" },
                    { 4, "Brasil" }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CountryId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "New York" },
                    { 2, 1, "Los Angeles" },
                    { 3, 2, "London" },
                    { 4, 2, "Manchester" },
                    { 5, 3, "Buenos Aires" },
                    { 6, 4, "Rio de Janeiro" }
                });

            migrationBuilder.InsertData(
                table: "Airports",
                columns: new[] { "Id", "CityId", "CodeIATA", "PublicName" },
                values: new object[,]
                {
                    { 1, 1, "JFK", "John F. Kennedy Intl" },
                    { 2, 2, "LAX", "Los Angeles Intl" },
                    { 3, 3, "LHR", "London Heathrow" },
                    { 4, 3, "LGW", "London Gatwick" },
                    { 5, 4, "MAN", "Manchester Airport" },
                    { 6, 1, "EWR", "Newark Liberty" },
                    { 7, 5, "EZE", "Aeropuerto Internacional Ezeiza" },
                    { 8, 6, "GIG", "Galeão–Antonio Carlos Jobim International Airport" }
                });

            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "Id", "AirportDestinationId", "AirportOriginId", "BaseFare", "CabinType", "FlightNumber", "ProviderName", "TimeArrival", "TimeDeparture" },
                values: new object[,]
                {
                    { 1, 2, 1, 150.00m, 0, "BW-99", "BudgetWings", new DateTime(2026, 6, 1, 13, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 8, 7, 200.00m, 2, "GA-101", "GlobalAir", new DateTime(2026, 6, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 2, 15, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Airports_CityId",
                table: "Airports",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Airports_CodeIATA",
                table: "Airports",
                column: "CodeIATA",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ReferenceCode",
                table: "Bookings",
                column: "ReferenceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirportDestinationId",
                table: "Flights",
                column: "AirportDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirportOriginId",
                table: "Flights",
                column: "AirportOriginId");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_BookingId",
                table: "Passengers",
                column: "BookingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
