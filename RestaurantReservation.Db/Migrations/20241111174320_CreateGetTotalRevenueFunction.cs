using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateGetTotalRevenueFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE FUNCTION GetTotalRevenue (
                                     @RestaurantId INT
                                   )
                                   RETURNS DECIMAL(18,3) 
                                   AS
                                   BEGIN
                                     DECLARE @TotalRevenue DECIMAL(18,3);

                                     SELECT @TotalRevenue=SUM(TotalAmount)
                                     FROM Reservations Rv
                                     JOIN Orders O ON Rv.ReservationId = O.ReservationId
                                     WHERE Rv.RestaurantId = @RestaurantId;

                                     RETURN ISNULL(@TotalRevenue, 0);
                                   END; 
                                   ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS  dbo.GetTotalRevenue");
        }
    }
}
