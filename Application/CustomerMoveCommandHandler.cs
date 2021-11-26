using CRM.Domain;
using FunicularSwitch;
using WebApi.HypermediaExtensions.Hypermedia.Actions;

namespace CRM.Application;

public class CustomerMoveCommandHandler
{
    private readonly ICustomerRepository m_CustomerRepository;

    public CustomerMoveCommandHandler(ICustomerRepository customerRepository)
    {
        m_CustomerRepository = customerRepository;
    }
    public async Task<Result<Customer>> Move(CustomerId customerId, Address newAddress)
    {
        var customerResult = await m_CustomerRepository.GetById(customerId);
        return await customerResult
            .Bind(c =>
           {
               c.Move(new Domain.Address(newAddress.Street, newAddress.City, newAddress.ZipCode, newAddress.Country));
               return Result.Ok(c);
           })
            .Bind(async c =>
            {
                await m_CustomerRepository.Update(c);
                return Result.Ok(c);
            });

    }
}
public record Address(string Street, string ZipCode, string City, string Country) : IHypermediaActionParameter;