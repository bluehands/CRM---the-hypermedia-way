using WebApi.HypermediaExtensions.Hypermedia;
using WebApi.HypermediaExtensions.Hypermedia.Attributes;

namespace CRM.Server.Controllers;

[HypermediaObject(Title = "tbd. Campaignes", Classes = new[] { "CampaignesRoot" })]
public class CampaignsRootHto : HypermediaObject
{
}