using WebApi.HypermediaExtensions.Hypermedia;
using WebApi.HypermediaExtensions.Hypermedia.Attributes;
using WebApi.HypermediaExtensions.Hypermedia.Links;

namespace CRM.Server.Controllers;

[HypermediaObject(Title = "Entry to the CRM REST API", Classes = new[] { "EntryPoint" })]
public class EntryPointHto : HypermediaObject
{
    public EntryPointHto()
    {
        Links.Add("customers", new HypermediaObjectKeyReference(typeof(CustomersRootHto)));
        Links.Add("campaigns", new HypermediaObjectKeyReference(typeof(CampaignsRootHto)));
    }
}