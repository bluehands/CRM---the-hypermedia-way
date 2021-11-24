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
        Links.Add("allCustomers", new HypermediaObjectKeyReference(typeof(CustomerQueryResultHto)));
        RegisterCustomer = new RegisterCustomer(() => true, () => { });
        CreateCustomersQuery = new CreateCustomersQuery(() => true, () => { });
    }

    [HypermediaAction(Name = "RegisterCustomer", Title = "Register a customer to our CRM.")]
    public RegisterCustomer RegisterCustomer { get; set; }

    [HypermediaAction(Name = "CreateCustomersQuery", Title = "Create a query to select customers")]
    public CreateCustomersQuery CreateCustomersQuery { get; set; }
}

public class CreateCustomersQuery:HypermediaAction
{
    public CreateCustomersQuery(Func<bool> canExecute, Action command) : base(canExecute, command)
    {
    }
}

public class RegisterCustomer : HypermediaAction
{
    public RegisterCustomer(Func<bool> canExecute, Action command) : base(canExecute, command)
    {
    }
}