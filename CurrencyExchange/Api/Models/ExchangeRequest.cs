namespace Api.Models;

public class ExchangeRequest
{
    public Guid IdempotencyKey { get; set; }
    public Guid UserId { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public decimal Amount { get; set; }
}