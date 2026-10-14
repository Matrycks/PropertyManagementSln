using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForRentalApplication : Migration
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
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Applicant",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Applicant_UserId",
                table: "Applicant",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicant_AspNetUsers_UserId",
                table: "Applicant",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalApplications_RentalUnits_RentalUnitId",
                table: "RentalApplications",
                column: "RentalUnitId",
                principalTable: "RentalUnits",
                principalColumn: "RentalUnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicant_AspNetUsers_UserId",
                table: "Applicant");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalApplications_RentalUnits_RentalUnitId",
                table: "RentalApplications");

            migrationBuilder.DropIndex(
                name: "IX_Applicant_UserId",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Applicant");

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
    }
}
