
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Migrations
{
    [DbContext(typeof(ExchangeDbContext))]
    [Migration("20231129155422_Init")]
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.Sql(@"
                CREATE TABLE public.users (
	                id uuid NOT NULL,
	                ""name"" varchar NOT NULL,
	                CONSTRAINT users_pkey PRIMARY KEY (id)
                );");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users",
                schema: "public");
        }
    }
}
