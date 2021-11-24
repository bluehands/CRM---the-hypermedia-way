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
        Links.Add("allCustomers", new HypermediaObjectKeyReference(typeof(CustomerQueryResultHto)));
        RegisterCustomer = new RegisterCustomer(() => true, _ => { });
        CreateCustomersQuery = new CreateCustomersQuery(() => true, _ => { });
    }

    [HypermediaAction(Name = "RegisterCustomer", Title = "Register a customer to our CRM.")]
    public RegisterCustomer RegisterCustomer { get; set; }

    [HypermediaAction(Name = "CreateCustomersQuery", Title = "Create a query to select customers")]
    public CreateCustomersQuery CreateCustomersQuery { get; set; }
}

public class CreateCustomersQuery : HypermediaAction<QueryParameter>
{
    public CreateCustomersQuery(Func<bool> canExecute, Action<QueryParameter> command) : base(canExecute, command)
    {
    }
}

public class RegisterCustomer : HypermediaAction<NewCustomerData>
{
    public RegisterCustomer(Func<bool> canExecute, Action<NewCustomerData> command) : base(canExecute, command)
    {
    }
}