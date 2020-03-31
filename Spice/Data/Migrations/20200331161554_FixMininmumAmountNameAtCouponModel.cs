using Microsoft.EntityFrameworkCore.Migrations;

namespace Spice.Data.Migrations
{
    public partial class FixMininmumAmountNameAtCouponModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumAccount",
                table: "Coupon");

            migrationBuilder.AddColumn<double>(
                name: "MinimumAmount",
                table: "Coupon",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumAmount",
                table: "Coupon");

            migrationBuilder.AddColumn<double>(
                name: "MinimumAccount",
                table: "Coupon",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
