using Bluehands.Hypermedia.Client.Hypermedia;
using Bluehands.Hypermedia.Client.Hypermedia.Attributes;
using Bluehands.Hypermedia.Client.Hypermedia.Commands;
using Bluehands.Hypermedia.Relations;

namespace CRM.CLI;

[HypermediaClientObject("Customer")]
public class CustomerHco : HypermediaClientObject
{
    [Mandatory]
    public string? Name { get; set; }
    
    public string? Street { get; set; }
   
    public string? ZipCode { get; set; }
   
    public string? City { get; set; }
    
    public string? Country { get; set; }
    [Mandatory]
    public bool? IsFavorite { get; set; }
    

    [HypermediaCommand("MarkAsFavorite")]
    public IHypermediaClientFunction<CustomerHco, FavoriteCustomerData>? MarkAsFavorite { get; set; }

    [HypermediaCommand("UnmarkAsFavorite")]
    public IHypermediaClientAction? UnmarkAsFavorite { get; set; }

    [Mandatory]
    [HypermediaCommand("Move")]
    public IHypermediaClientFunction<CustomerHco, Address>? Move { get; set; }

    [Mandatory]
    [HypermediaRelations(new[] { DefaultHypermediaRelations.Self })]
    public MandatoryHypermediaLink<CustomerHco>? Self { get; set; }
}
public record FavoriteCustomerData(string Url) ;
public record Address(string Street, string ZipCode, string City, string Country) ;