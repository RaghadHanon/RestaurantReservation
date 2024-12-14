using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateReservationWithCustomerAndRestaurantView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW ReservationWithCustomerAndRestaurant
                                   AS
                                   SELECT 
                                     Rv.ReservationId,
                                     Rv.ReservationDate,
                                     Rv.PartySize,
                                     Rs.Name AS RestaurantName,
                                     Rs.Address AS RestaurantAddress,
                                     Rs.PhoneNumber AS RestaurantPhoneNumber,
                                     Rs.OpeningHours,
                                     C.FirstName + ' ' + C.LastName AS CustomerName,
                                     C.Email AS CustomerEmail,
                                     C.PhoneNumber AS CustomerPhoneNumber
                                   FROM 
                                     Restaurants Rs
                                   JOIN 
                                     Reservations Rv ON Rs.RestaurantId = Rv.RestaurantId
                                   JOIN 
                                     Customers C ON C.CustomerId = Rv.CustomerId;
                                   ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS ReservationWithCustomerAndRestaurant;");
        }
    }
}
