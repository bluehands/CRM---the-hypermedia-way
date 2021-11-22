namespace CRM.Domain;

public record NewCustomerData(string Name, string Street, string ZipCode, string City, string Country);
public record FavoriteCustomerData(string Url);