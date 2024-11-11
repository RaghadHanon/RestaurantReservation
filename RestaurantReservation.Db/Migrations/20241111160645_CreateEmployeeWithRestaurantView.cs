using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateEmployeeWithRestaurantView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW EmployeeWithRestaurant
                                   AS
                                   SELECT 
                                     Rs.Name AS RestaurantName,
                                     Rs.Address AS RestaurantAddress,
                                     Rs.PhoneNumber AS RestaurantPhoneNumber,
                                     Rs.OpeningHours,
                                     E.FirstName + ' ' + E.LastName AS EmployeeName,
                                     E.Position 
                                   FROM 
                                     Restaurants Rs
                                   JOIN 
                                     Employees E ON Rs.RestaurantId = E.RestaurantId;
                                   ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW EmployeeWithRestaurant");
        }
    }
}
