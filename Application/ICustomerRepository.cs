using CRM.Domain;
using FunicularSwitch;

namespace CRM.Application;

public interface ICustomerRepository
{
    Task<Result<IEnumerable<Customer>>> GetAll();
    Task<Result<IEnumerable<Customer>>> GetAllFavorites();
    Task<Result<Customer>> GetById(CustomerId customerId);
    Task<Result<Unit>> Update(Customer customer);
}