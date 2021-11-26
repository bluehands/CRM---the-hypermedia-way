using Bluehands.Hypermedia.Client.Hypermedia;
using Bluehands.Hypermedia.Client.Hypermedia.Attributes;
using Bluehands.Hypermedia.Relations;
using CRM.Server.Controllers;

namespace CRM.CLI;


[HypermediaClientObject("EntryPoint")]
public class EntryPointHco : HypermediaClientObject
{
    [Mandatory]
    [HypermediaRelations(new[] { DefaultHypermediaRelations.Self })]
    public MandatoryHypermediaLink<EntryPointHco>? Self { get; set; }

    [Mandatory]
    [HypermediaRelations(new[] { "customers" })]    
    public MandatoryHypermediaLink<CustomersRootHco>? Customers { get; set; }
    [Mandatory]
    [HypermediaRelations(new[] { "campaigns" })]
    public MandatoryHypermediaLink<CampaignsRootHco>? Campaigns { get; set; }

}