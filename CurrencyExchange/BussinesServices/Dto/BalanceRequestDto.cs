namespace BussinesServices.Dto
{
    public class BalanceRequestDto
    {
        public Guid UserId { get; set; }
        public string? CurrencyId { get; set;}
    }

    public class CreateBalanceRequestDto : BalanceRequestDto
    {
        public decimal Balance { get; set; }
    }
}
