using Microsoft.AspNetCore.Mvc;
using WebApi.HypermediaExtensions.WebApi.AttributedRoutes;

namespace CRM.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CampaignsRootController : Controller
{
    public CampaignsRootController(ILogger<CustomersController> logger)
    {
    }
    [HttpGetHypermediaObject(typeof(CampaignsRootHto))]
    public ActionResult Get()
    {
        return Ok(new CampaignsRootHto());
    }
}