using Bluehands.Hypermedia.Client.Hypermedia;
using Bluehands.Hypermedia.Client.Hypermedia.Attributes;
using Bluehands.Hypermedia.Relations;

namespace CRM.CLI;

[HypermediaClientObject("CustomerQueryResult")]
public class CustomerQueryResultHco : HypermediaClientObject
{
    [Mandatory]
    [HypermediaRelations(new[] { DefaultHypermediaRelations.Self })]
    public MandatoryHypermediaLink<CustomerQueryResultHco>? Self { get; set; }

    [HypermediaRelations(new[] { DefaultHypermediaRelations.EmbeddedEntities.Item })] 
    public List<CustomerHco>? Customers { get; set; }


}