using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CarRepairServiceCode.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    client_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    first_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    last_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    wallet = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true, defaultValueSql: "0"),
                    mobile_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.client_id);
                });

            migrationBuilder.CreateTable(
                name: "emp_position",
                columns: table => new
                {
                    position_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    position_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("empposition_pkey", x => x.position_id);
                });

            migrationBuilder.CreateTable(
                name: "task_catalog",
                columns: table => new
                {
                    task_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    task_name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    task_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    task_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true, defaultValueSql: "0"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("taskcatalog_pkey", x => x.task_id);
                });

            migrationBuilder.CreateTable(
                name: "car",
                columns: table => new
                {
                    car_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    car_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    vin_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    colour = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_car", x => x.car_id);
                    table.ForeignKey(
                        name: "fk_client_car",
                        column: x => x.client_id,
                        principalTable: "client",
                        principalColumn: "client_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    first_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    last_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    position_id = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    emp_login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    emp_password = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.employee_id);
                    table.ForeignKey(
                        name: "fk_employee_position",
                        column: x => x.position_id,
                        principalTable: "emp_position",
                        principalColumn: "position_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "car_order",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    car_id = table.Column<int>(type: "integer", nullable: false),
                    order_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    order_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true, defaultValueSql: "0"),
                    order_comments = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("carorder_pkey", x => x.order_id);
                    table.ForeignKey(
                        name: "fk_carorder_carid",
                        column: x => x.car_id,
                        principalTable: "car",
                        principalColumn: "car_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "car_order_details",
                columns: table => new
                {
                    order_details_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    task_id = table.Column<int>(type: "integer", nullable: false),
                    employee_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("carorderdetails_pkey", x => x.order_details_id);
                    table.ForeignKey(
                        name: "fk_carorderdetails_carorder",
                        column: x => x.order_id,
                        principalTable: "car_order",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_carorderdetails_employee",
                        column: x => x.employee_id,
                        principalTable: "employee",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_carorderdetails_taskcatalog",
                        column: x => x.task_id,
                        principalTable: "task_catalog",
                        principalColumn: "task_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_car_client_id",
                table: "car",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_car_order_car_id",
                table: "car_order",
                column: "car_id");

            migrationBuilder.CreateIndex(
                name: "IX_car_order_details_employee_id",
                table: "car_order_details",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_car_order_details_order_id",
                table: "car_order_details",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_car_order_details_task_id",
                table: "car_order_details",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_position_id",
                table: "employee",
                column: "position_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "car_order_details");

            migrationBuilder.DropTable(
                name: "car_order");

            migrationBuilder.DropTable(
                name: "employee");

            migrationBuilder.DropTable(
                name: "task_catalog");

            migrationBuilder.DropTable(
                name: "car");

            migrationBuilder.DropTable(
                name: "emp_position");

            migrationBuilder.DropTable(
                name: "client");
        }
    }
}
