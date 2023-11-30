namespace BussinesServices.Dto
{
    public class BalanceRequestDto
    {
        public Guid UserId { get; set; }
        public string? CurrencyId { get; set;}
        public decimal Balance { get; set; }
    }
}
