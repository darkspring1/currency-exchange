
using Dal.Entities;
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

            migrationBuilder.Sql(@$"
        CREATE TABLE public.users (
	        id uuid primary key,
	        name varchar({User.MaxNameLen}) not null
        );

        CREATE TABLE public.currencies (
            id varchar(5) primary key,
            name varchar({Currency.MaxNameLen}) not null
        );

        CREATE TABLE public.accounts (
            user_id uuid not null,
            currency_id varchar(5) not null,
            balance decimal(18,6) not null,
 
            primary key (user_id, currency_id),
            foreign key (user_id) references public.users(id),
            foreign key (currency_id) references public.currencies(id)
        );

        CREATE TABLE public.exchange_history (
            id uuid primary key,
            user_id uuid not null,
            from_currency_id varchar(5) not null,
            to_currency_id varchar(5) not null,
            rate decimal(10,10) not null,
            fee decimal(4,4) not null,
            fee_amount decimal(18,6) not null,
            from_amount decimal(18,6) not null,
            to_amount decimal(18,6) not null
        );");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
