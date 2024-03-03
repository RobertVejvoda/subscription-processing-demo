using Microsoft.OpenApi.Extensions;

namespace CustomerService.Domain;

public class Customer : IAggregateRoot
{
    public string Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public CustomerState State { get; private set; }

    public int Age => Core.Helpers.Calculator.CalculateAge(BirthDate);

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

    public Dto.Customer ToDto() 
        => new(Id, FirstName, LastName, BirthDate, Email, State.GetDisplayName(), Age);

    public static Customer FromDto(Dto.Customer dto) =>
        new(dto.CustomerId, 
            dto.FirstName,
            dto.LastName,
            dto.BirthDate,
            dto.Email,
            Enum.Parse<CustomerState>(dto.CustomerState));

    public static string GetId(string email, DateOnly birthDate)
    {
        var input = $"{birthDate.ToString("yyyy-MM-dd")}|{email.Trim().ToLowerInvariant()}";
        var bytes = Encoding.UTF8.GetBytes(input);
        var base64 = Convert.ToBase64String(bytes).ToLowerInvariant();
        return base64.TrimEnd('=');
    }
}