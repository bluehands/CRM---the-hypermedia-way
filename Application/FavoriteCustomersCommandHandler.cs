using CRM.Domain;
using FunicularSwitch;

namespace CRM.Application;

public class FavoriteCustomersCommandHandler
{
    private readonly ICustomerRepository m_CustomerRepository;

    public FavoriteCustomersCommandHandler(ICustomerRepository customerRepository)
    {
        m_CustomerRepository = customerRepository;
    }

    public async Task<Result<IEnumerable<Customer>>> GetAllCustomers()
    {
        return await m_CustomerRepository.GetAllFavorites();
    }

    public async Task<Result<Customer>> MarkAsFavorite(Guid id)
    {
        var customerResult = await m_CustomerRepository.GetById(new CustomerId(id));
        return customerResult.Bind<Customer>(c => c.ExecuteMarkAsFavorite());
    }
    public async Task<Result<Customer>> UnMarkAsFavorite(Guid id)
    {
        var customerResult = await m_CustomerRepository.GetById(new CustomerId(id));
        return customerResult.Bind<Customer>(c => c.ExecuteUnMarkAsFavorite());
    }
}