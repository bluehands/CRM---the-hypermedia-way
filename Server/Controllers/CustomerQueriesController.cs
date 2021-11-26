using CRM.Application;
using Microsoft.AspNetCore.Mvc;
using WebApi.HypermediaExtensions.WebApi.ExtensionMethods;

namespace CRM.Server.Controllers;

[Route("api/customers/queries")]
[ApiController]
public class CustomerQueriesController : Controller
{
    private readonly ProblemFactory m_ProblemFactory;
    private readonly CustomerCommandHandler m_CustomerCommandHandler;

    public CustomerQueriesController(ILogger<CustomersController> logger, ProblemFactory problemFactory, CustomerCommandHandler customerCommandHandler)
    {
        m_ProblemFactory = problemFactory;
        m_CustomerCommandHandler = customerCommandHandler;
    }
    [HttpGet(Name ="CustomerQuery")]
    public async Task<IActionResult> Query(string? name, string? country)
    {
        var queryResult = await m_CustomerCommandHandler.QueryCustomers(new QueryParameter(name, country));
        return queryResult.Match<IActionResult>(
            allCustomers => Ok(allCustomers),
            e => this.Problem(m_ProblemFactory.Exception(e)));
    }
}