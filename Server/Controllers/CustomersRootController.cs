using Microsoft.AspNetCore.Mvc;
using WebApi.HypermediaExtensions.WebApi.AttributedRoutes;

namespace CRM.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersRootController : Controller
{
    public CustomersRootController(ILogger<CustomersController> logger)
    {
    }
    [HttpGetHypermediaObject(typeof(CustomersRootHto))]
    public ActionResult Get()
    {
        return Ok(new CustomersRootHto());
    }
}