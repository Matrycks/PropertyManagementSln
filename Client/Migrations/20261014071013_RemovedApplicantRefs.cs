using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Migrations
{
    /// <inheritdoc />
    public partial class RemovedApplicantRefs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalApplications_Applicant_ApplicantId",
                table: "RentalApplications");

            migrationBuilder.DropIndex(
                name: "IX_RentalApplications_ApplicantId",
                table: "RentalApplications");

            migrationBuilder.DropColumn(
                name: "ApplicantId",
                table: "RentalApplications");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "RentalApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "RentalApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DateOfBirth",
                table: "RentalApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "RentalApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "RentalApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "RentalApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "RentalApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "RentalApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "RentalApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "RentalApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "RentalApplications");

            migrationBuilder.DropColumn(
                name: "City",
                table: "RentalApplications");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "RentalApplications");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "RentalApplications");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "RentalApplications");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "RentalApplications");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "RentalApplications");

            migrationBuilder.DropColumn(
                name: "State",
                table: "RentalApplications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RentalApplications");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "RentalApplications");

            migrationBuilder.AddColumn<int>(
                name: "ApplicantId",
                table: "RentalApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RentalApplications_ApplicantId",
                table: "RentalApplications",
                column: "ApplicantId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalApplications_Applicant_ApplicantId",
                table: "RentalApplications",
                column: "ApplicantId",
                principalTable: "Applicant",
                principalColumn: "ApplicantId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
