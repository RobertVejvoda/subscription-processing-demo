namespace CustomerService.Model;

public record CustomerIdModel([Required] string CustomerId)
{
    public static CustomerIdModel Empty => new("");
}