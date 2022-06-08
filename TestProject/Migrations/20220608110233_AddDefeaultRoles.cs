using Microsoft.EntityFrameworkCore.Migrations;

namespace TestProject.Migrations
{
    public partial class AddDefeaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3a116c14-d148-4a26-aba5-b340c7b37bd1", "1fdd6c5b-f424-4cbf-a864-37f2e0cf2e11", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "71d3e409-4be4-4fb1-9cde-8485c5adc417", "68afdb7f-4e29-4427-8ccd-cdf7e7e50e93", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3a116c14-d148-4a26-aba5-b340c7b37bd1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "71d3e409-4be4-4fb1-9cde-8485c5adc417");
        }
    }
}
