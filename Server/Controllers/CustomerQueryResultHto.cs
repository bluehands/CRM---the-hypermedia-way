using Bluehands.Hypermedia.Relations;
using CRM.Domain;
using WebApi.HypermediaExtensions.Hypermedia;
using WebApi.HypermediaExtensions.Hypermedia.Attributes;
using WebApi.HypermediaExtensions.Hypermedia.Links;

namespace CRM.Server.Controllers;

[HypermediaObject(Title = "A list of customers", Classes = new[] { "CustomerQueryResult" })]
public class CustomerQueryResultHto : HypermediaObject
{
    public CustomerQueryResultHto(IEnumerable<Customer> customers)
    {
        var entities = customers.Select(c => new RelatedEntity(DefaultHypermediaRelations.EmbeddedEntities.Item, new HypermediaObjectReference(new CustomerHto(c))));
        Entities.AddRange(entities);
    }
}