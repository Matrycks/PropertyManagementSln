using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Migrations
{
    /// <inheritdoc />
    public partial class AddResidenceHistoryRefToApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentalApplicationId",
                table: "ResidenceHistory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResidenceHistory_RentalApplicationId",
                table: "ResidenceHistory",
                column: "RentalApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResidenceHistory_RentalApplications_RentalApplicationId",
                table: "ResidenceHistory",
                column: "RentalApplicationId",
                principalTable: "RentalApplications",
                principalColumn: "RentalApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResidenceHistory_RentalApplications_RentalApplicationId",
                table: "ResidenceHistory");

            migrationBuilder.DropIndex(
                name: "IX_ResidenceHistory_RentalApplicationId",
                table: "ResidenceHistory");

            migrationBuilder.DropColumn(
                name: "RentalApplicationId",
                table: "ResidenceHistory");
        }
    }
}
