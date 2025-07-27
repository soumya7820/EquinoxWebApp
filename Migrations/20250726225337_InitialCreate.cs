using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Equinox.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassCategory",
                columns: table => new
                {
                    ClassCategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassCategory", x => x.ClassCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Club",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Club", x => x.ClubId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    DOB = table.Column<string>(type: "TEXT", nullable: false),
                    IsCoach = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "EquinoxClass",
                columns: table => new
                {
                    EquinoxClassId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ClassPicture = table.Column<string>(type: "TEXT", nullable: false),
                    ClassDay = table.Column<string>(type: "TEXT", nullable: false),
                    Time = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ClassCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClubId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquinoxClass", x => x.EquinoxClassId);
                    table.ForeignKey(
                        name: "FK_EquinoxClass_ClassCategory_ClassCategoryId",
                        column: x => x.ClassCategoryId,
                        principalTable: "ClassCategory",
                        principalColumn: "ClassCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquinoxClass_Club_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquinoxClass_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ClassCategory",
                columns: new[] { "ClassCategoryId", "Name" },
                values: new object[,]
                {
                    { 1, "Boxing" },
                    { 2, "Yoga" },
                    { 3, "HIIT" },
                    { 4, "Strength" },
                    { 5, "Dancing" }
                });

            migrationBuilder.InsertData(
                table: "Club",
                columns: new[] { "ClubId", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Chicago Loop", "312-111-1111" },
                    { 2, "West Chicago", "312-222-2222" },
                    { 3, "Lincoln Park", "312-333-3333" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "DOB", "Email", "IsCoach", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "07/08/2000", "john.smith@equinox.com", true, "John Smith", "555-000-0001" },
                    { 2, "07/08/2001", "emily.johnson@equinox.com", true, "Emily Johnson", "555-000-0002" }
                });

            migrationBuilder.InsertData(
                table: "EquinoxClass",
                columns: new[] { "EquinoxClassId", "ClassCategoryId", "ClassDay", "ClassPicture", "ClubId", "Name", "Time", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "Monday", "boxing.png", 1, "Boxing 101", "8 AM – 9 AM", 1 },
                    { 2, 2, "Wednesday", "yoga.png", 2, "Hatha Yoga", "6 PM – 7 PM", 2 },
                    { 3, 3, "Friday", "hiit.png", 3, "HIIT Junior", "5 PM – 6 PM", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquinoxClass_ClassCategoryId",
                table: "EquinoxClass",
                column: "ClassCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EquinoxClass_ClubId",
                table: "EquinoxClass",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_EquinoxClass_UserId",
                table: "EquinoxClass",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquinoxClass");

            migrationBuilder.DropTable(
                name: "ClassCategory");

            migrationBuilder.DropTable(
                name: "Club");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
