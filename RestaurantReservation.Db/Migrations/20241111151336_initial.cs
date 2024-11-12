using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 11, 11, 10, 13, 36, 519, DateTimeKind.Utc).AddTicks(9649));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2024, 11, 11, 5, 13, 36, 519, DateTimeKind.Utc).AddTicks(9653));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 3,
                column: "OrderDate",
                value: new DateTime(2024, 11, 11, 7, 13, 36, 519, DateTimeKind.Utc).AddTicks(9654));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 4,
                column: "OrderDate",
                value: new DateTime(2024, 11, 11, 12, 13, 36, 519, DateTimeKind.Utc).AddTicks(9655));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 5,
                column: "OrderDate",
                value: new DateTime(2024, 11, 11, 9, 13, 36, 519, DateTimeKind.Utc).AddTicks(9656));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "ReservationId",
                keyValue: 1,
                column: "ReservationDate",
                value: new DateTime(2024, 11, 12, 15, 13, 36, 519, DateTimeKind.Utc).AddTicks(9621));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "ReservationId",
                keyValue: 2,
                column: "ReservationDate",
                value: new DateTime(2024, 11, 13, 15, 13, 36, 519, DateTimeKind.Utc).AddTicks(9629));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "ReservationId",
                keyValue: 3,
                column: "ReservationDate",
                value: new DateTime(2024, 11, 14, 15, 13, 36, 519, DateTimeKind.Utc).AddTicks(9630));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "ReservationId",
                keyValue: 4,
                column: "ReservationDate",
                value: new DateTime(2024, 11, 15, 15, 13, 36, 519, DateTimeKind.Utc).AddTicks(9631));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "ReservationId",
                keyValue: 5,
                column: "ReservationDate",
                value: new DateTime(2024, 11, 16, 15, 13, 36, 519, DateTimeKind.Utc).AddTicks(9632));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 11, 11, 10, 11, 23, 158, DateTimeKind.Utc).AddTicks(8426));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2024, 11, 11, 5, 11, 23, 158, DateTimeKind.Utc).AddTicks(8431));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 3,
                column: "OrderDate",
                value: new DateTime(2024, 11, 11, 7, 11, 23, 158, DateTimeKind.Utc).AddTicks(8432));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 4,
                column: "OrderDate",
                value: new DateTime(2024, 11, 11, 12, 11, 23, 158, DateTimeKind.Utc).AddTicks(8433));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 5,
                column: "OrderDate",
                value: new DateTime(2024, 11, 11, 9, 11, 23, 158, DateTimeKind.Utc).AddTicks(8434));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "ReservationId",
                keyValue: 1,
                column: "ReservationDate",
                value: new DateTime(2024, 11, 12, 15, 11, 23, 158, DateTimeKind.Utc).AddTicks(8392));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "ReservationId",
                keyValue: 2,
                column: "ReservationDate",
                value: new DateTime(2024, 11, 13, 15, 11, 23, 158, DateTimeKind.Utc).AddTicks(8404));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "ReservationId",
                keyValue: 3,
                column: "ReservationDate",
                value: new DateTime(2024, 11, 14, 15, 11, 23, 158, DateTimeKind.Utc).AddTicks(8406));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "ReservationId",
                keyValue: 4,
                column: "ReservationDate",
                value: new DateTime(2024, 11, 15, 15, 11, 23, 158, DateTimeKind.Utc).AddTicks(8407));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "ReservationId",
                keyValue: 5,
                column: "ReservationDate",
                value: new DateTime(2024, 11, 16, 15, 11, 23, 158, DateTimeKind.Utc).AddTicks(8408));
        }
    }
}
