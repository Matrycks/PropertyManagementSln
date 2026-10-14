using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Migrations
{
    /// <inheritdoc />
    public partial class AddedUnitRefToApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalApplications_RentalUnits_RentalUnitId",
                table: "RentalApplications");

            migrationBuilder.AlterColumn<int>(
                name: "RentalUnitId",
                table: "RentalApplications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalApplications_RentalUnits_RentalUnitId",
                table: "RentalApplications",
                column: "RentalUnitId",
                principalTable: "RentalUnits",
                principalColumn: "RentalUnitId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalApplications_RentalUnits_RentalUnitId",
                table: "RentalApplications");

            migrationBuilder.AlterColumn<int>(
                name: "RentalUnitId",
                table: "RentalApplications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalApplications_RentalUnits_RentalUnitId",
                table: "RentalApplications",
                column: "RentalUnitId",
                principalTable: "RentalUnits",
                principalColumn: "RentalUnitId");
        }
    }
}
