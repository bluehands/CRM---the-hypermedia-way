using Bluehands.Hypermedia.Client.Hypermedia;
using Bluehands.Hypermedia.Client.Hypermedia.Attributes;
using Bluehands.Hypermedia.Client.Hypermedia.Commands;
using Bluehands.Hypermedia.Relations;
using CRM.CLI;

namespace CRM.Server.Controllers;

[HypermediaClientObject("CustomersRoot" )]
public class CustomersRootHco : HypermediaClientObject
{
    [Mandatory]
    [HypermediaRelations(new[] { DefaultHypermediaRelations.Self })]
    public MandatoryHypermediaLink<CustomersRootHco>? Self { get; set; }

    [Mandatory]
    [HypermediaRelations(new[] { "allCustomers" })]
    public MandatoryHypermediaLink<CustomerQueryResultHco>? AllCustomers { get; set; }
    [Mandatory]
    [HypermediaRelations(new[] { "favorites" })]
    public MandatoryHypermediaLink<CustomerQueryResultHco>? Favorites { get; set; }

    [Mandatory]
    [HypermediaCommand("RegisterCustomer")]
    public IHypermediaClientFunction<CustomerHco, NewCustomerData>? RegisterCustomer { get; set; }
    [Mandatory]
    [HypermediaCommand("CreateCustomersQuery")]
    public IHypermediaClientFunction<CustomerQueryResultHco, QueryParameter>? CreateCustomersQuery { get; set; }


}
public record NewCustomerData(string Name, string Street, string ZipCode, string City, string Country);
public record QueryParameter(string? Name, string? Country, bool? IsFavorite);