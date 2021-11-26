using CRM.Application;
using WebApi.HypermediaExtensions.Hypermedia;
using WebApi.HypermediaExtensions.Hypermedia.Actions;
using WebApi.HypermediaExtensions.Hypermedia.Attributes;
using WebApi.HypermediaExtensions.Hypermedia.Links;

namespace CRM.Server.Controllers;

[HypermediaObject(Title = "Customers API Root", Classes = new[] { "CustomersRoot" })]
public class CustomersRootHto : HypermediaObject
{
    public CustomersRootHto()
    {
        Links.Add("allCustomers", new HypermediaObjectQueryReference(typeof(CustomerQueryResultHto), new QueryParameter(string.Empty, string.Empty, null)));
        RegisterCustomer = new RegisterCustomer(() => true, _ => { });
    }

    [HypermediaAction(Name = "RegisterCustomer", Title = "Register a customer to our CRM.")]
    public RegisterCustomer RegisterCustomer { get; set; }
}

public class RegisterCustomer : HypermediaAction<NewCustomerData>
{
    public RegisterCustomer(Func<bool> canExecute, Action<NewCustomerData> command) : base(canExecute, command)
    {
    }
}