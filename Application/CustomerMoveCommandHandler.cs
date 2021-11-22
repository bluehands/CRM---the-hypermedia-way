using CRM.Domain;
using FunicularSwitch;

namespace CRM.Application
{
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
                   c.Move(newAddress);
                   return Result.Ok(c);
               })
                .Bind(async c =>
                {
                    await m_CustomerRepository.Update(c);
                    return Result.Ok(c);
                });

        }
    }
}