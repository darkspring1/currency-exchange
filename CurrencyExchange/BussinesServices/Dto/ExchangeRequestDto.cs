namespace BussinesServices.Dto;

public class ExchangeRequestDto
{
    public Guid IdempotencyKey { get; set; }
    public Guid UserId { get; set; }

    private readonly string _from; 
    public string From
    {
        get => _from;
        init => _from = value.ToUpper();
    }
    
    private readonly string _to;
    public string To
    {
        get => _to;
        init => _to = value.ToUpper();
    }
    public decimal Amount { get; set; }
    public decimal Fee { get; set; }
    public decimal Rate { get; set; }
}
