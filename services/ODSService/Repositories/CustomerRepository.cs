namespace ODSService.Repositories;

public class CustomerRepository
{
    private readonly ODSDataContext context;

    public CustomerRepository(ODSDataContext context)
    {
        this.context = context;
    }

    public async Task<Customer?> GetById(string customerId)
    {
        return await context.Customers.FindAsync(customerId);
    }

    public async Task Add(Customer customer)
    {
        await context.AddAsync(customer);
    }

    public void Update(Customer entity)
    {
        if (context.Entry(entity).State == EntityState.Detached)
        {
            context.Customers.Attach(entity);
        }
        context.Entry(entity).State = EntityState.Modified;
    }
    
    public async Task Remove(string customerId)
    {
        var customer = await GetById(customerId);
        if (customer == null)
            throw new InvalidOperationException();
        
        context.Remove(customer);
    }
    
    public int Execute(string sql, params object[] parameters)
    {
        return context.Database.ExecuteSqlRaw(sql, parameters);
    }
}