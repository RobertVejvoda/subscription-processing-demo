namespace CustomerService.Model;

public class Customer : IAggregateRoot
{
    public string Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public CustomerState State { get; private set; }

    public Customer(string firstName, string lastName, DateOnly birthDate, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email.Trim().ToLowerInvariant();
        BirthDate = birthDate;
        State = CustomerState.New;
        Id = ToBase64String();
    }

    private Customer(string id, string firstName, string lastName, DateOnly birthDate, string email,
        CustomerState state)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        Email = email;
        State = state;
    }

    public Customer Activate()
    {
        State = CustomerState.Active;
        return this;
    }

    public Customer Deactivate()
    {
        State = CustomerState.Inactive;
        return this;
    }

    public CustomerModel ToModel() 
        => new(Id, FirstName, LastName, BirthDate, Email, State.Name);

    public static Customer FromModel(CustomerModel model) =>
        new(model.CustomerId, 
            model.FirstName,
            model.LastName,
            model.BirthDate,
            model.Email,
            Enumeration.FromDisplayName<CustomerState>(model.CustomerState));

    private string ToBase64String() => Convert.ToBase64String(Encoding.UTF8.GetBytes(Id), Base64FormattingOptions.None);

    public static string GetId(string email, DateOnly birthDate) => Convert.ToBase64String(
        Encoding.UTF8.GetBytes($"{email.Trim().ToLowerInvariant()}|{birthDate.ToString("yyyyMMdd")}"));
    public static string FromBase64String(string base64String) => Encoding.UTF8.GetString(Convert.FromBase64String(base64String));
}