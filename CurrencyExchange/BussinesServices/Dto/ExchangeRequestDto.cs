namespace BussinesServices.Dto;

public class ExchangeRequestDto
{
    public Guid IdempotencyKey { get; set; }
    public Guid UserId { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public decimal Amount { get; set; }
    public decimal Fee { get; set; }
    public decimal Rate { get; set; }
}
