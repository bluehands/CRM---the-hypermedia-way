using Bluehands.Hypermedia.Relations;
using CRM.Application;
using CRM.Domain;
using Microsoft.AspNetCore.Mvc;
using WebApi.HypermediaExtensions.Hypermedia;
using WebApi.HypermediaExtensions.Hypermedia.Actions;
using WebApi.HypermediaExtensions.Hypermedia.Links;
using WebApi.HypermediaExtensions.WebApi.AttributedRoutes;
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


    [HttpGetHypermediaObject(typeof(CustomerQueryResultHto), Name = "CustomerQuery")]
    public async Task<IActionResult> Query(string? name, string? country, bool? isFavorite)
    {
        var queryResult = await m_CustomerCommandHandler.QueryCustomers(new QueryParameter(name, country, isFavorite));
        return queryResult.Match<IActionResult>(
            allCustomers => Ok(new CustomerQueryResultHto(allCustomers)),
            e => this.Problem(m_ProblemFactory.Exception(e)));
    }
    [HttpPostHypermediaAction(typeof(CreateCustomersQuery))]
    public IActionResult Post([FromBody] QueryParameter value)
    {
        if (string.IsNullOrEmpty(value.Country) && string.IsNullOrEmpty(value.Name))
        {
            return this.Problem(m_ProblemFactory.Exception("Name or Country must be given"));
        }
        var newQueryUrl = Url.Link("CustomerQuery", new { name = value.Name, country = value.Country });
        return Created(newQueryUrl ?? string.Empty, null);
    }

}

public class CustomerQueryResultHto : HypermediaObject
{
    public CustomerQueryResultHto(IEnumerable<Customer> customers)
    {
        var entities = customers.Select(c => new RelatedEntity(DefaultHypermediaRelations.EmbeddedEntities.Item, new HypermediaObjectReference(new CustomerHto(c))));
        Entities.AddRange(entities);
    }
}