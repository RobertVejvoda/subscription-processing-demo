namespace CustomerService.Model;

public class CustomerState(int id, string name) : Enumeration(id, name)
{
    public static CustomerState New = new CustomerState(1, "New");
    public static CustomerState Active = new CustomerState(2, "Active");
    public static CustomerState Inactive = new CustomerState(3, "Inactive");
    public static CustomerState Forbidden = new CustomerState(4, "Forbidden");
}