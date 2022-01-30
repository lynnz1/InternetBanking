using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace s3512958_a2.Migrations
{
    public partial class BlockLoginAndBillpay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Login",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "BillPay",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Login");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "BillPay");
        }
    }
}
