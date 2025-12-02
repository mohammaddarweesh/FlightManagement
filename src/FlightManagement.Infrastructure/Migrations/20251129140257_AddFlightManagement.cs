using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFlightManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IataCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    IcaoCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LogoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IataCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    IcaoCode = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Timezone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric(10,7)", precision: 10, scale: 7, nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric(10,7)", precision: 10, scale: 7, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IconUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Aircraft",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AirlineId = table.Column<Guid>(type: "uuid", nullable: false),
                    Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Manufacturer = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TotalSeats = table.Column<int>(type: "integer", nullable: false),
                    ManufactureYear = table.Column<int>(type: "integer", nullable: true),
                    RangeKm = table.Column<int>(type: "integer", nullable: true),
                    CruisingSpeedKmh = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircraft", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aircraft_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AircraftCabinClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AircraftId = table.Column<Guid>(type: "uuid", nullable: false),
                    CabinClass = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SeatCount = table.Column<int>(type: "integer", nullable: false),
                    RowStart = table.Column<int>(type: "integer", nullable: false),
                    RowEnd = table.Column<int>(type: "integer", nullable: false),
                    SeatsPerRow = table.Column<int>(type: "integer", nullable: false),
                    SeatLayout = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BasePriceMultiplier = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 1.0m),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftCabinClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AircraftCabinClasses_Aircraft_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircraft",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FlightNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AirlineId = table.Column<Guid>(type: "uuid", nullable: false),
                    AircraftId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartureAirportId = table.Column<Guid>(type: "uuid", nullable: false),
                    ArrivalAirportId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScheduledDepartureTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ScheduledArrivalTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActualDepartureTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualArrivalTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DepartureTerminal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ArrivalTerminal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DepartureGate = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ArrivalGate = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Scheduled"),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    DistanceKm = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flights_Aircraft_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircraft",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flights_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flights_Airports_ArrivalAirportId",
                        column: x => x.ArrivalAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flights_Airports_DepartureAirportId",
                        column: x => x.DepartureAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AircraftId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeatNumber = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Row = table.Column<int>(type: "integer", nullable: false),
                    Column = table.Column<char>(type: "character(1)", maxLength: 1, nullable: false),
                    CabinClass = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SeatType = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IsEmergencyExit = table.Column<bool>(type: "boolean", nullable: false),
                    HasExtraLegroom = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_Aircraft_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircraft",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightAmenities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FlightId = table.Column<Guid>(type: "uuid", nullable: false),
                    AmenityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CabinClass = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsIncluded = table.Column<bool>(type: "boolean", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightAmenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightAmenities_Amenities_AmenityId",
                        column: x => x.AmenityId,
                        principalTable: "Amenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightAmenities_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FlightId = table.Column<Guid>(type: "uuid", nullable: false),
                    CabinClass = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BasePrice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, defaultValue: "USD"),
                    TaxAmount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    TotalSeats = table.Column<int>(type: "integer", nullable: false),
                    AvailableSeats = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightPricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightPricings_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightSeats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FlightId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false, defaultValue: "Available"),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    LockedUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LockedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    BookingId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightSeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightSeats_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightSeats_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aircraft_AirlineId",
                table: "Aircraft",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Aircraft_IsActive",
                table: "Aircraft",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Aircraft_RegistrationNumber",
                table: "Aircraft",
                column: "RegistrationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AircraftCabinClasses_AircraftId_CabinClass",
                table: "AircraftCabinClasses",
                columns: new[] { "AircraftId", "CabinClass" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airlines_IataCode",
                table: "Airlines",
                column: "IataCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airlines_IcaoCode",
                table: "Airlines",
                column: "IcaoCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airlines_IsActive",
                table: "Airlines",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Airports_City",
                table: "Airports",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_Airports_Country",
                table: "Airports",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_Airports_IataCode",
                table: "Airports",
                column: "IataCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airports_IcaoCode",
                table: "Airports",
                column: "IcaoCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airports_IsActive",
                table: "Airports",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_Category",
                table: "Amenities",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_Code",
                table: "Amenities",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_IsActive",
                table: "Amenities",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_FlightAmenities_AmenityId",
                table: "FlightAmenities",
                column: "AmenityId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightAmenities_FlightId",
                table: "FlightAmenities",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightAmenities_FlightId_AmenityId_CabinClass",
                table: "FlightAmenities",
                columns: new[] { "FlightId", "AmenityId", "CabinClass" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlightAmenities_IsIncluded",
                table: "FlightAmenities",
                column: "IsIncluded");

            migrationBuilder.CreateIndex(
                name: "IX_FlightPricings_AvailableSeats",
                table: "FlightPricings",
                column: "AvailableSeats");

            migrationBuilder.CreateIndex(
                name: "IX_FlightPricings_FlightId_CabinClass",
                table: "FlightPricings",
                columns: new[] { "FlightId", "CabinClass" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlightPricings_IsActive",
                table: "FlightPricings",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AircraftId",
                table: "Flights",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirlineId_ScheduledDepartureTime",
                table: "Flights",
                columns: new[] { "AirlineId", "ScheduledDepartureTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_ArrivalAirportId",
                table: "Flights",
                column: "ArrivalAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DepartureAirportId_ArrivalAirportId_ScheduledDepart~",
                table: "Flights",
                columns: new[] { "DepartureAirportId", "ArrivalAirportId", "ScheduledDepartureTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FlightNumber",
                table: "Flights",
                column: "FlightNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_IsActive",
                table: "Flights",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_ScheduledDepartureTime",
                table: "Flights",
                column: "ScheduledDepartureTime");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_Status",
                table: "Flights",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSeats_BookingId",
                table: "FlightSeats",
                column: "BookingId",
                filter: "\"BookingId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSeats_FlightId_SeatId",
                table: "FlightSeats",
                columns: new[] { "FlightId", "SeatId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlightSeats_FlightId_Status",
                table: "FlightSeats",
                columns: new[] { "FlightId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_FlightSeats_LockedUntil",
                table: "FlightSeats",
                column: "LockedUntil",
                filter: "\"LockedUntil\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSeats_SeatId",
                table: "FlightSeats",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_AircraftId_CabinClass",
                table: "Seats",
                columns: new[] { "AircraftId", "CabinClass" });

            migrationBuilder.CreateIndex(
                name: "IX_Seats_AircraftId_SeatNumber",
                table: "Seats",
                columns: new[] { "AircraftId", "SeatNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_IsActive",
                table: "Seats",
                column: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AircraftCabinClasses");

            migrationBuilder.DropTable(
                name: "FlightAmenities");

            migrationBuilder.DropTable(
                name: "FlightPricings");

            migrationBuilder.DropTable(
                name: "FlightSeats");

            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropTable(
                name: "Aircraft");

            migrationBuilder.DropTable(
                name: "Airlines");
        }
    }
}
