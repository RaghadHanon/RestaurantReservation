using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateFindCustomersByPartySizeStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE FindCustomersByPartySize
                                     @PartySize INT
                                   AS
                                   BEGIN
                                     SELECT DISTINCT C.*
                                     FROM
                                     Customers C
                                     JOIN Reservations R ON R.CustomerId = C.CustomerId
                                     WHERE R.PartySize > @PartySize;
                                   END;"
                                 );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS FindCustomersByPartySize");
        }
    }
}
