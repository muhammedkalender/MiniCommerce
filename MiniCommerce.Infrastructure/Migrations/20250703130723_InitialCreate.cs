using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_orders", x => x.Id);
                });
            
             migrationBuilder.InsertData(
                table: "tbl_orders",
                columns: new[] { "Id", "UserId", "ProductId", "Quantity", "PaymentMethod", "CreatedAt" },
                values: new object[,]
                {
                    {
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
                        Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0001"),
                        1, 0, DateTime.UtcNow.AddDays(-1)
                    },
                    {
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0002"),
                        Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
                        2, 1, DateTime.UtcNow.AddDays(-2)
                    },
                    {
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0003"),
                        Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0003"),
                        3, 0, DateTime.UtcNow.AddDays(-3)
                    },
                    {
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0004"),
                        Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0004"),
                        1, 0, DateTime.UtcNow.AddDays(-4)
                    },
                    {
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0005"),
                        Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0005"),
                        4, 1, DateTime.UtcNow.AddDays(-5)
                    },
                    {
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0006"),
                        Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0006"),
                        2, 0, DateTime.UtcNow.AddDays(-6)
                    },
                    {
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0007"),
                        Guid.Parse("44444444-4444-4444-4444-444444444444"),
                        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0007"),
                        5, 0, DateTime.UtcNow.AddDays(-7)
                    },
                    {
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0008"),
                        Guid.Parse("44444444-4444-4444-4444-444444444444"),
                        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0008"),
                        3, 1, DateTime.UtcNow.AddDays(-8)
                    },
                    {
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0009"),
                        Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0009"),
                        2, 1, DateTime.UtcNow.AddDays(-9)
                    },
                    {
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0010"),
                        Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0010"),
                        1, 0, DateTime.UtcNow.AddDays(-10)
                    } 
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_orders");
        }
    }
}
