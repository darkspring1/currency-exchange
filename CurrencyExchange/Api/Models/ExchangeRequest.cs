namespace Api.Models;

public class ExchangeRequest
{
    public string From { get; set; }
    public string To { get; set; }
    public decimal Amount { get; set; }
}