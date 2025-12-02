using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessLogicAndReportingEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PromotionId",
                table: "Bookings",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlackoutDates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BlocksBookings = table.Column<bool>(type: "boolean", nullable: false),
                    BlocksPromotions = table.Column<bool>(type: "boolean", nullable: false),
                    AirlineId = table.Column<Guid>(type: "uuid", nullable: true),
                    DepartureAirportId = table.Column<Guid>(type: "uuid", nullable: true),
                    ArrivalAirportId = table.Column<Guid>(type: "uuid", nullable: true),
                    CabinClass = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackoutDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlackoutDates_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BlackoutDates_Airports_ArrivalAirportId",
                        column: x => x.ArrivalAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BlackoutDates_Airports_DepartureAirportId",
                        column: x => x.DepartureAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BookingPolicies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    SecondaryValue = table.Column<int>(type: "integer", nullable: true),
                    ErrorMessage = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    AirlineId = table.Column<Guid>(type: "uuid", nullable: true),
                    DepartureAirportId = table.Column<Guid>(type: "uuid", nullable: true),
                    ArrivalAirportId = table.Column<Guid>(type: "uuid", nullable: true),
                    CabinClass = table.Column<int>(type: "integer", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingPolicies_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BookingPolicies_Airports_ArrivalAirportId",
                        column: x => x.ArrivalAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BookingPolicies_Airports_DepartureAirportId",
                        column: x => x.DepartureAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DynamicPricingRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    RuleType = table.Column<int>(type: "integer", nullable: false),
                    AdjustmentPercentage = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: false),
                    FixedAdjustment = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    ApplicableDays = table.Column<int>(type: "integer", nullable: true),
                    SeasonType = table.Column<int>(type: "integer", nullable: true),
                    SeasonStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SeasonEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MinBookingPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    MaxBookingPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    MinDaysBeforeDeparture = table.Column<int>(type: "integer", nullable: true),
                    MaxDaysBeforeDeparture = table.Column<int>(type: "integer", nullable: true),
                    StartHour = table.Column<int>(type: "integer", nullable: true),
                    EndHour = table.Column<int>(type: "integer", nullable: true),
                    AirlineId = table.Column<Guid>(type: "uuid", nullable: true),
                    DepartureAirportId = table.Column<Guid>(type: "uuid", nullable: true),
                    ArrivalAirportId = table.Column<Guid>(type: "uuid", nullable: true),
                    CabinClass = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicPricingRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicPricingRules_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DynamicPricingRules_Airports_ArrivalAirportId",
                        column: x => x.ArrivalAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DynamicPricingRules_Airports_DepartureAirportId",
                        column: x => x.DepartureAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "OverbookingPolicies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AirlineId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    MaxOverbookingPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    MaxOverbookedSeats = table.Column<int>(type: "integer", nullable: true),
                    CabinClass = table.Column<int>(type: "integer", nullable: true),
                    DepartureAirportId = table.Column<Guid>(type: "uuid", nullable: true),
                    ArrivalAirportId = table.Column<Guid>(type: "uuid", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverbookingPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OverbookingPolicies_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OverbookingPolicies_Airports_ArrivalAirportId",
                        column: x => x.ArrivalAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_OverbookingPolicies_Airports_DepartureAirportId",
                        column: x => x.DepartureAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    DiscountType = table.Column<int>(type: "integer", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MaxDiscountAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MinBookingAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MaxTotalUses = table.Column<int>(type: "integer", nullable: true),
                    MaxUsesPerCustomer = table.Column<int>(type: "integer", nullable: true),
                    CurrentUsageCount = table.Column<int>(type: "integer", nullable: false),
                    ApplicableRoutes = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    ApplicableCabinClasses = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ApplicableDays = table.Column<int>(type: "integer", nullable: false),
                    ApplicableAirlineIds = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    FirstTimeCustomersOnly = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeasonalPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    SeasonType = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AdjustmentPercentage = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: false),
                    AirlineId = table.Column<Guid>(type: "uuid", nullable: true),
                    DepartureAirportId = table.Column<Guid>(type: "uuid", nullable: true),
                    ArrivalAirportId = table.Column<Guid>(type: "uuid", nullable: true),
                    CabinClass = table.Column<int>(type: "integer", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonalPricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeasonalPricings_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SeasonalPricings_Airports_ArrivalAirportId",
                        column: x => x.ArrivalAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SeasonalPricings_Airports_DepartureAirportId",
                        column: x => x.DepartureAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PromotionUsages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PromotionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    BookingId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    UsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionUsages_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PromotionUsages_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PromotionUsages_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PromotionId",
                table: "Bookings",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_BlackoutDates_AirlineId",
                table: "BlackoutDates",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_BlackoutDates_ArrivalAirportId",
                table: "BlackoutDates",
                column: "ArrivalAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_BlackoutDates_DepartureAirportId",
                table: "BlackoutDates",
                column: "DepartureAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_BlackoutDates_EndDate",
                table: "BlackoutDates",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_BlackoutDates_IsActive",
                table: "BlackoutDates",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_BlackoutDates_StartDate",
                table: "BlackoutDates",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_BlackoutDates_StartDate_EndDate",
                table: "BlackoutDates",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BookingPolicies_AirlineId",
                table: "BookingPolicies",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPolicies_ArrivalAirportId",
                table: "BookingPolicies",
                column: "ArrivalAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPolicies_Code",
                table: "BookingPolicies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingPolicies_DepartureAirportId",
                table: "BookingPolicies",
                column: "DepartureAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPolicies_IsActive",
                table: "BookingPolicies",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPolicies_Priority",
                table: "BookingPolicies",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPolicies_Type",
                table: "BookingPolicies",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicPricingRules_AirlineId",
                table: "DynamicPricingRules",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicPricingRules_ArrivalAirportId",
                table: "DynamicPricingRules",
                column: "ArrivalAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicPricingRules_DepartureAirportId",
                table: "DynamicPricingRules",
                column: "DepartureAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicPricingRules_IsActive",
                table: "DynamicPricingRules",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicPricingRules_Priority",
                table: "DynamicPricingRules",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicPricingRules_RuleType",
                table: "DynamicPricingRules",
                column: "RuleType");

            migrationBuilder.CreateIndex(
                name: "IX_OverbookingPolicies_AirlineId",
                table: "OverbookingPolicies",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_OverbookingPolicies_ArrivalAirportId",
                table: "OverbookingPolicies",
                column: "ArrivalAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_OverbookingPolicies_DepartureAirportId",
                table: "OverbookingPolicies",
                column: "DepartureAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_OverbookingPolicies_IsActive",
                table: "OverbookingPolicies",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_OverbookingPolicies_Priority",
                table: "OverbookingPolicies",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_Code",
                table: "Promotions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_IsActive",
                table: "Promotions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_Status",
                table: "Promotions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_Type",
                table: "Promotions",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_ValidFrom",
                table: "Promotions",
                column: "ValidFrom");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_ValidTo",
                table: "Promotions",
                column: "ValidTo");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionUsages_BookingId",
                table: "PromotionUsages",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionUsages_CustomerId",
                table: "PromotionUsages",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionUsages_PromotionId",
                table: "PromotionUsages",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionUsages_PromotionId_CustomerId",
                table: "PromotionUsages",
                columns: new[] { "PromotionId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_SeasonalPricings_AirlineId",
                table: "SeasonalPricings",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonalPricings_ArrivalAirportId",
                table: "SeasonalPricings",
                column: "ArrivalAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonalPricings_DepartureAirportId",
                table: "SeasonalPricings",
                column: "DepartureAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonalPricings_EndDate",
                table: "SeasonalPricings",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonalPricings_IsActive",
                table: "SeasonalPricings",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonalPricings_SeasonType",
                table: "SeasonalPricings",
                column: "SeasonType");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonalPricings_StartDate",
                table: "SeasonalPricings",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonalPricings_StartDate_EndDate",
                table: "SeasonalPricings",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Promotions_PromotionId",
                table: "Bookings",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Promotions_PromotionId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "BlackoutDates");

            migrationBuilder.DropTable(
                name: "BookingPolicies");

            migrationBuilder.DropTable(
                name: "DynamicPricingRules");

            migrationBuilder.DropTable(
                name: "OverbookingPolicies");

            migrationBuilder.DropTable(
                name: "PromotionUsages");

            migrationBuilder.DropTable(
                name: "SeasonalPricings");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_PromotionId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "Bookings");
        }
    }
}
