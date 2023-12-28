using Core.Domain;
using Core.Types;

namespace CustomerService.Model;

public class Customer : IAggregateRoot
{
    public string Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public CustomerState State { get; private set; }

    public Customer(string firstName, string lastName, string email, DateOnly birthDate, string id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        BirthDate = birthDate;
        State = CustomerState.New;
        Id = Guid.NewGuid().ToString("n");
    }

    private Customer(string id, string firstName, string lastName, DateOnly birthDate, string email,
        CustomerState state)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
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
        => new CustomerModel(Id, FirstName, LastName, Email, BirthDate, State.Name);

    public static Customer FromModel(CustomerModel model) =>
        new(model.Id, 
            model.FirstName,
            model.LastName,
            model.BirthDate,
            model.Email,
            Enumeration.FromDisplayName<CustomerState>(model.State));
}