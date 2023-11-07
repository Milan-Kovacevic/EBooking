using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBooking.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "UnitFeature",
                columns: table => new
                {
                    FeatureId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitFeature", x => x.FeatureId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Administrator",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrator", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Administrator_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Employee_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accommodation",
                columns: table => new
                {
                    AccommodationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodation", x => x.AccommodationId);
                    table.ForeignKey(
                        name: "FK_Accommodation_Administrator_UserId",
                        column: x => x.UserId,
                        principalTable: "Administrator",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accommodation_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightClass = table.Column<int>(type: "INTEGER", nullable: false),
                    AvioCompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    TicketPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    FlightCapacity = table.Column<int>(type: "INTEGER", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    FromLocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    ToLocationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight", x => x.FlightId);
                    table.ForeignKey(
                        name: "FK_Flight_Administrator_UserId",
                        column: x => x.UserId,
                        principalTable: "Administrator",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flight_Location_FromLocationId",
                        column: x => x.FromLocationId,
                        principalTable: "Location",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flight_Location_ToLocationId",
                        column: x => x.ToLocationId,
                        principalTable: "Location",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripReservation",
                columns: table => new
                {
                    TripId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OnName = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfSeats = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripReservation", x => x.TripId);
                    table.ForeignKey(
                        name: "FK_TripReservation_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationUnit",
                columns: table => new
                {
                    UnitId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    AvailableFrom = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AvailableTo = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NumberOfBeds = table.Column<int>(type: "INTEGER", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "TEXT", nullable: false),
                    AccommodationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationUnit", x => x.UnitId);
                    table.ForeignKey(
                        name: "FK_AccommodationUnit_Accommodation_AccommodationId",
                        column: x => x.AccommodationId,
                        principalTable: "Accommodation",
                        principalColumn: "AccommodationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightOnTripReservation",
                columns: table => new
                {
                    TripId = table.Column<int>(type: "INTEGER", nullable: false),
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightOnTripReservation", x => new { x.TripId, x.FlightId });
                    table.ForeignKey(
                        name: "FK_FlightOnTripReservation_Flight_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flight",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightOnTripReservation_TripReservation_TripId",
                        column: x => x.TripId,
                        principalTable: "TripReservation",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationUnitReservation",
                columns: table => new
                {
                    UnitReservationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitId = table.Column<int>(type: "INTEGER", nullable: false),
                    OnName = table.Column<string>(type: "TEXT", nullable: false),
                    ReservationFrom = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReservationTo = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NumberOfAdults = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfChildren = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationUnitReservation", x => x.UnitReservationId);
                    table.ForeignKey(
                        name: "FK_AccommodationUnitReservation_AccommodationUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "AccommodationUnit",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccommodationUnitReservation_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeatureOnUnit",
                columns: table => new
                {
                    FeatureId = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureOnUnit", x => new { x.FeatureId, x.UnitId });
                    table.ForeignKey(
                        name: "FK_FeatureOnUnit_AccommodationUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "AccommodationUnit",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureOnUnit_UnitFeature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "UnitFeature",
                        principalColumn: "FeatureId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accommodation_AccommodationId",
                table: "Accommodation",
                column: "AccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodation_LocationId",
                table: "Accommodation",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodation_UserId",
                table: "Accommodation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationUnit_AccommodationId",
                table: "AccommodationUnit",
                column: "AccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationUnit_UnitId",
                table: "AccommodationUnit",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationUnitReservation_EmployeeId",
                table: "AccommodationUnitReservation",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationUnitReservation_UnitId",
                table: "AccommodationUnitReservation",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationUnitReservation_UnitReservationId",
                table: "AccommodationUnitReservation",
                column: "UnitReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Administrator_UserId",
                table: "Administrator",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_UserId",
                table: "Employee",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeatureOnUnit_UnitId",
                table: "FeatureOnUnit",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_FlightId",
                table: "Flight",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_FromLocationId",
                table: "Flight",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_ToLocationId",
                table: "Flight",
                column: "ToLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_UserId",
                table: "Flight",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightOnTripReservation_FlightId",
                table: "FlightOnTripReservation",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_LocationId",
                table: "Location",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TripReservation_EmployeeId",
                table: "TripReservation",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TripReservation_TripId",
                table: "TripReservation",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitFeature_FeatureId",
                table: "UnitFeature",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserId",
                table: "User",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccommodationUnitReservation");

            migrationBuilder.DropTable(
                name: "FeatureOnUnit");

            migrationBuilder.DropTable(
                name: "FlightOnTripReservation");

            migrationBuilder.DropTable(
                name: "AccommodationUnit");

            migrationBuilder.DropTable(
                name: "UnitFeature");

            migrationBuilder.DropTable(
                name: "Flight");

            migrationBuilder.DropTable(
                name: "TripReservation");

            migrationBuilder.DropTable(
                name: "Accommodation");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Administrator");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
