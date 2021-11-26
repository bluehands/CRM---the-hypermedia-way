using CRM.Application;
using CRM.Domain;
using Microsoft.AspNetCore.Mvc;
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

    }

}
