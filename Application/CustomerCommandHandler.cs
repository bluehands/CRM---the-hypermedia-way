using CRM.Domain;
using FunicularSwitch;
using WebApi.HypermediaExtensions.Hypermedia.Actions;
using WebApi.HypermediaExtensions.Query;

namespace CRM.Application
{
    public class CustomerCommandHandler
    {
        private readonly ICustomerRepository m_CustomerRepository;

        public CustomerCommandHandler(ICustomerRepository customerRepository)
        {
            m_CustomerRepository = customerRepository;
        }

        public async Task<Result<IEnumerable<Customer>>> GetAllCustomers()
        {
            return await m_CustomerRepository.GetAll();
        }
        public async Task<Result<IEnumerable<Customer>>> QueryCustomers(QueryParameter parameters)
        {
            return await m_CustomerRepository.GetAll().Bind(allCustomers =>
            {
                IEnumerable<Customer> queryResult = allCustomers;
                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    queryResult = queryResult.Where(c => c.Name.Contains(parameters.Name, StringComparison.InvariantCultureIgnoreCase));
                }

                if (!string.IsNullOrEmpty(parameters.Country))
                {
                    queryResult = queryResult.Where(c => c.Country.Contains(parameters.Country, StringComparison.InvariantCultureIgnoreCase));
                }
                if (parameters.IsFavorite != null)
                {
                    queryResult = queryResult.Where(c => c.IsFavorite == parameters.IsFavorite!);
                }
                return Result.Ok<IEnumerable<Customer>>(queryResult);
            });
        }

        public async Task<Result<Customer>> GetCustomerById(Guid id)
        {
            return await m_CustomerRepository.GetById(new CustomerId(id));
        }

        public async Task<Result<Customer>> AddCustomer(NewCustomerData newCustomerData)
        {
            var customer = new Customer(new CustomerId(Guid.NewGuid()), newCustomerData.Name, newCustomerData.Street, newCustomerData.ZipCode, newCustomerData.City, newCustomerData.Country, String.Empty);
            await m_CustomerRepository.Update(customer);
            return Result.Ok<Customer>(customer);
        }
    }
    public record NewCustomerData(string Name, string Street, string ZipCode, string City, string Country) : IHypermediaActionParameter;
    public record QueryParameter(string? Name, string? Country, bool? IsFavorite) : IHypermediaQuery, IHypermediaActionParameter;
}