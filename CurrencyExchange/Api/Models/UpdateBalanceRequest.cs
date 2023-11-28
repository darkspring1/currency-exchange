namespace Api.Models;

public class UpdateBalanceRequest
{
    public string Currency { get; set; }
    public decimal Amount { get; set; }
}