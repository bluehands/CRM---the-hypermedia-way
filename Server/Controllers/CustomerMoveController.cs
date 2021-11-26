using CRM.Application;
using CRM.Domain;
using Microsoft.AspNetCore.Mvc;
using WebApi.HypermediaExtensions.WebApi.AttributedRoutes;
using WebApi.HypermediaExtensions.WebApi.ExtensionMethods;

namespace CRM.Server.Controllers
{
    [Route("api/customers/{id}/moves")]
    [ApiController]
    public class CustomerMoveController : Controller
    {
        private readonly ProblemFactory m_ProblemFactory;
        private readonly CustomerMoveCommandHandler m_CustomerMoveCommandHandler;

        public CustomerMoveController(ILogger<CustomersController> logger, ProblemFactory problemFactory, CustomerMoveCommandHandler customerMoveCommandHandler)
        {
            m_ProblemFactory = problemFactory;
            m_CustomerMoveCommandHandler = customerMoveCommandHandler;
        }

        [HttpPostHypermediaAction(typeof(Move))]
        public async Task<IActionResult> Move(Guid id, [FromBody] Application.Address value)
        {
            var customerResult = await m_CustomerMoveCommandHandler.Move(new CustomerId(id), value);
            return customerResult.Match<IActionResult>(
                customer =>
                {
                    var newCustomerUrl = Url.Link("GetCustomerById", new { id = customer.Id.Value.ToString() });
                    return Created(newCustomerUrl ?? string.Empty, null);
                },
                e => this.Problem(m_ProblemFactory.Exception(e)));
        }

    }
}
