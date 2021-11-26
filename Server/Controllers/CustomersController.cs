using CRM.Application;
using CRM.Domain;
using Microsoft.AspNetCore.Mvc;
using WebApi.HypermediaExtensions.WebApi.ExtensionMethods;

namespace CRM.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : Controller
{
    private readonly ProblemFactory m_ProblemFactory;
    private readonly CustomerCommandHandler m_CustomerCommandHandler;
    private readonly CustomerMoveCommandHandler m_CustomerMoveCommandHandler;

    public CustomersController(ILogger<CustomersController> logger, ProblemFactory problemFactory, CustomerCommandHandler customerCommandHandler, CustomerMoveCommandHandler customerMoveCommandHandler)
    {
        m_ProblemFactory = problemFactory;
        m_CustomerCommandHandler = customerCommandHandler;
        m_CustomerMoveCommandHandler = customerMoveCommandHandler;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var customersResult = await m_CustomerCommandHandler.GetAllCustomers();
        return customersResult.Match<IActionResult>(
            allCustomers => Ok(allCustomers),
            e => this.Problem(m_ProblemFactory.Exception(e)));
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public async Task<IActionResult> Get(Guid id)
    {
        var customerResult = await m_CustomerCommandHandler.GetCustomerById(id);
        return customerResult.Match<IActionResult>(
            customer => Ok(customer),
            e => this.Problem(m_ProblemFactory.Exception(e)));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] NewCustomerData value)
    {
        throw new NotImplementedException();
    }

}