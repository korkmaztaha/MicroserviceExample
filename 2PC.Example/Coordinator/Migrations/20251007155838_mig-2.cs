using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Coordinator.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("59ae36da-a681-40e9-9656-9872d98d1c33"), "Payment.API" },
                    { new Guid("7a8f1af9-c6d2-413f-b6a2-71775ea9403a"), "Order.API" },
                    { new Guid("82e6db3f-e8ec-47f9-aef2-bcfef8d64ee1"), "Stock.API" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("59ae36da-a681-40e9-9656-9872d98d1c33"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("7a8f1af9-c6d2-413f-b6a2-71775ea9403a"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("82e6db3f-e8ec-47f9-aef2-bcfef8d64ee1"));
        }
    }
}
