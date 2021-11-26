﻿using CRM.Domain;
using WebApi.HypermediaExtensions.Hypermedia;
using WebApi.HypermediaExtensions.Hypermedia.Actions;
using WebApi.HypermediaExtensions.Hypermedia.Attributes;
using WebApi.HypermediaExtensions.WebApi.RouteResolver;

namespace CRM.Server.Controllers;

[HypermediaObject(Title = "Customer", Classes = new[] { "Customer" })]
public class CustomerHto : HypermediaObject
{

    [Key]
    [FormatterIgnoreHypermediaProperty]
    public Guid Id { get; }
    public string Name { get; }
    public string Street { get; }
    public string ZipCode { get; }
    public string City { get; }
    public string Country { get; }
    public bool IsFavorite { get; }

    public CustomerHto(Customer customer)
    {
        Id = customer.Id.Value;
        Name = customer.Name;
        Street = customer.Street;
        ZipCode = customer.ZipCode;
        City = customer.City;
        Country = customer.Country;
        IsFavorite = customer.IsFavorite;
        MarkAsFavorite = new MarkAsFavorite(() => !IsFavorite, _ => { });
        UnmarkAsFavorite = new UnmarkAsFavorite(() => IsFavorite, () => { });
        Move = new Move(() => true, _ => { });
    }

    [HypermediaAction(Name = "MarkAsFavorite", Title = "Mark the customer as favorite")]
    public MarkAsFavorite MarkAsFavorite { get; set; }

    [HypermediaAction(Name = "UnmarkAsFavorite", Title = "Unmark the customer as favorite")]
    public UnmarkAsFavorite UnmarkAsFavorite { get; set; }

    [HypermediaAction(Name = "Move", Title = "Customer has moved. Set the new address")]
    public Move Move { get; set; }
}

public class Move : HypermediaAction<CRM.Application.Address>
{
    public Move(Func<bool> canExecute, Action<CRM.Application.Address> command) : base(canExecute, command)
    {
    }
}

public class MarkAsFavorite : HypermediaAction<FavoriteCustomerData>
{
    public MarkAsFavorite(Func<bool> canExecute, Action<FavoriteCustomerData> command) : base(canExecute, command)
    {
    }
}
public class UnmarkAsFavorite : HypermediaAction
{
    public UnmarkAsFavorite(Func<bool> canExecute, Action command) : base(canExecute, command)
    {
    }
}