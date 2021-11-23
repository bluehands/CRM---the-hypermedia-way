using Microsoft.AspNetCore.Mvc;
using WebApi.HypermediaExtensions.WebApi.AttributedRoutes;

namespace CRM.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EntryPointController : Controller
{
    [HttpGetHypermediaObject(typeof(EntryPointHto))]
    public ActionResult GetEntryPoint()
    {
        return Ok(new EntryPointHto());
    }
}