using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertyRefToApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentalPropertyId",
                table: "RentalApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RentalApplications_RentalPropertyId",
                table: "RentalApplications",
                column: "RentalPropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalApplications_RentalProperties_RentalPropertyId",
                table: "RentalApplications",
                column: "RentalPropertyId",
                principalTable: "RentalProperties",
                principalColumn: "RentalPropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalApplications_RentalProperties_RentalPropertyId",
                table: "RentalApplications");

            migrationBuilder.DropIndex(
                name: "IX_RentalApplications_RentalPropertyId",
                table: "RentalApplications");

            migrationBuilder.DropColumn(
                name: "RentalPropertyId",
                table: "RentalApplications");
        }
    }
}
