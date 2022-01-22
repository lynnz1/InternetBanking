using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace s3512958_a2.Migrations
{
    public partial class ModifiedAnnotation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Postcode",
                table: "Customer",
                newName: "PostCode");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Customer",
                newName: "Suburb");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Transaction",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<int>(
                name: "NumOfTransactions",
                table: "Account",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumOfTransactions",
                table: "Account");

            migrationBuilder.RenameColumn(
                name: "PostCode",
                table: "Customer",
                newName: "Postcode");

            migrationBuilder.RenameColumn(
                name: "Suburb",
                table: "Customer",
                newName: "City");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Transaction",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);
        }
    }
}
