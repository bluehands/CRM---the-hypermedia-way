using CRM.Application;
using CRM.Domain;
using Microsoft.AspNetCore.Mvc;
using WebApi.HypermediaExtensions.WebApi.ExtensionMethods;

namespace CRM.Server.Controllers;

[Route("api/customers/favorites")]
[ApiController]
public class FavoriteCustomersController : Controller
{
    private readonly ProblemFactory m_ProblemFactory;
    private readonly FavoriteCustomersCommandHandler m_FavoriteCustomersCommandHandler;

    public FavoriteCustomersController(ILogger<CustomersController> logger, ProblemFactory problemFactory, FavoriteCustomersCommandHandler favoriteCustomersCommandHandler)
    {
        m_ProblemFactory = problemFactory;
        m_FavoriteCustomersCommandHandler = favoriteCustomersCommandHandler;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var customersResult = await m_FavoriteCustomersCommandHandler.GetAllCustomers();
        return customersResult.Match<IActionResult>(
            allCustomers => Ok(allCustomers),
            e => this.Problem(m_ProblemFactory.Exception(e)));
    }
}

