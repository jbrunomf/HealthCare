using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.Data.Migrations
{
    /// <inheritdoc />
    public partial class medical0schedule2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MedicalScheduleId",
                table: "Appointments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_MedicalScheduleId",
                table: "Appointments",
                column: "MedicalScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_MedicalSchedules_MedicalScheduleId",
                table: "Appointments",
                column: "MedicalScheduleId",
                principalTable: "MedicalSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_MedicalSchedules_MedicalScheduleId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_MedicalScheduleId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "MedicalScheduleId",
                table: "Appointments");
        }
    }
}
