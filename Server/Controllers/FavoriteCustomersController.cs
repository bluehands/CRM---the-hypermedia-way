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
    [HttpPost]
    public async Task<IActionResult> MarkAsFavorite([FromBody] FavoriteCustomerData value)
    {
        var uri = new Uri(value.Url);
        var id = uri.Segments.LastOrDefault();
        var customerResult = await m_FavoriteCustomersCommandHandler.MarkAsFavorite(Guid.Parse(id ?? string.Empty));
        return customerResult.Match<IActionResult>(
            customer => 
            {
                var newCustomerUrl = Url.Link("GetCustomerById", new { id = customer.Id.Value.ToString() });
                return Created(newCustomerUrl ?? string.Empty, null);
            },
            e => this.Problem(m_ProblemFactory.Exception(e)));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> UnMarkAsFavorite(Guid id)
    {
        var customerResult = await m_FavoriteCustomersCommandHandler.UnMarkAsFavorite(id);
        return customerResult.Match<IActionResult>(
            _ => NoContent(),
            e => this.Problem(m_ProblemFactory.Exception(e)));
    }
}

