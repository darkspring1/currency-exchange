
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
	        id uuid primary key,
	        name varchar not null
        );

        CREATE TABLE public.currencies (
            id varchar(5) primary key,
            name varchar not null
        );

        CREATE TABLE public.accounts (
            user_id uuid not null,
            currency_id varchar(5) not null,
            balance decimal(18,6) not null,
            name VARCHAR not null,
 
            primary key (user_id, currency_id),
            foreign key (user_id) references public.users(id),
            foreign key (currency_id) references public.currencies(id)
        );

        CREATE TABLE public.exchange_rates (
            currency_from_id varchar(5) not null,
            currency_to_id varchar(5) not null,
            rate decimal(10,8) not null,

            primary key (currency_from_id, currency_to_id),
            foreign key (currency_from_id) references public.currencies(id),
            foreign key (currency_to_id) references public.currencies(id)
        );

        CREATE TABLE public.exchange_history (
            id uuid primary key,
            user_id uuid not null,
            currency_from_id varchar(5) not null,
            currency_to_id varchar(5) not null,
            rate decimal(10,8) not null,
            amount decimal(18,6) not null
        );

");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
