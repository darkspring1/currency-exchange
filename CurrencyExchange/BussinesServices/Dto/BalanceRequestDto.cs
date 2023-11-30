namespace BussinesServices.Dto
{
    public class BalanceRequestDto
    {
        public Guid UserId { get; init; }
        public string? CurrencyId { get; init;}
    }

    public class CreateBalanceRequestDto : BalanceRequestDto
    {
        public decimal Balance { get; init; }
    }
}
