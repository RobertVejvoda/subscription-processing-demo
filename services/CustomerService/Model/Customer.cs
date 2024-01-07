using Microsoft.OpenApi.Extensions;

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
        Id = GetId(Email, BirthDate);
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
        => new(Id, FirstName, LastName, BirthDate, Email, State.GetDisplayName(), DateTime.Now.Year - BirthDate.Year);

    public static Customer FromModel(CustomerModel model) =>
        new(model.CustomerId, 
            model.FirstName,
            model.LastName,
            model.BirthDate,
            model.Email,
            Enum.Parse<CustomerState>(model.CustomerState));

    public static string GetId(string email, DateOnly birthDate)
    {
        var input = $"{birthDate.ToString("yyyy-MM-dd")}|{email.Trim().ToLowerInvariant()}";
        var bytes = Encoding.UTF8.GetBytes(input);
        var base64 = Convert.ToBase64String(bytes);
        return base64.TrimEnd('=');
    }
}