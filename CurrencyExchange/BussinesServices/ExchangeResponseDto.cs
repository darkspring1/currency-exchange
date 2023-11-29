namespace BussinesServices;

public class ExchangeResponseDto
{
    public Guid IdempotencyKey { get; set; }
    public Guid UserId { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public decimal FromAmount { get; set; }
    public decimal ToAmount { get; set; }
    public decimal Fee { get; set; }
    public decimal FeeAmount { get; set; }
}