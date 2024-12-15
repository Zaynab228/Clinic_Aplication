using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clinic_api_project.Migrations
{
    public partial class t60 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_apointments_AspNetUsers_DoctorID",
                table: "apointments");

            migrationBuilder.DropForeignKey(
                name: "FK_apointments_AspNetUsers_patientID",
                table: "apointments");

            migrationBuilder.DropForeignKey(
                name: "FK_medicicals_AspNetUsers_DoctorID",
                table: "medicicals");

            migrationBuilder.DropForeignKey(
                name: "FK_medicicals_AspNetUsers_patientID",
                table: "medicicals");

            migrationBuilder.AddForeignKey(
                name: "FK_apointments_AspNetUsers_DoctorID",
                table: "apointments",
                column: "DoctorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_apointments_AspNetUsers_patientID",
                table: "apointments",
                column: "patientID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_medicicals_AspNetUsers_DoctorID",
                table: "medicicals",
                column: "DoctorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_medicicals_AspNetUsers_patientID",
                table: "medicicals",
                column: "patientID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_apointments_AspNetUsers_DoctorID",
                table: "apointments");

            migrationBuilder.DropForeignKey(
                name: "FK_apointments_AspNetUsers_patientID",
                table: "apointments");

            migrationBuilder.DropForeignKey(
                name: "FK_medicicals_AspNetUsers_DoctorID",
                table: "medicicals");

            migrationBuilder.DropForeignKey(
                name: "FK_medicicals_AspNetUsers_patientID",
                table: "medicicals");

            migrationBuilder.AddForeignKey(
                name: "FK_apointments_AspNetUsers_DoctorID",
                table: "apointments",
                column: "DoctorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_apointments_AspNetUsers_patientID",
                table: "apointments",
                column: "patientID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_medicicals_AspNetUsers_DoctorID",
                table: "medicicals",
                column: "DoctorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_medicicals_AspNetUsers_patientID",
                table: "medicicals",
                column: "patientID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
