using FunicularSwitch;

namespace CRM.Domain
{
    public class Customer
    {
        public CustomerId Id { get; }
        public string Name { get; }
        public string Street { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }
        public string PictureUrl { get; }
        public bool IsFavorite { get; private set; }

        public Customer(CustomerId id, string name, string street, string zipCode, string city, string country, string pictureUrl)
        {
            Id = id;
            Name = name;
            Street = street;
            ZipCode = zipCode;
            City = city;
            Country = country;
            PictureUrl = pictureUrl;
            IsFavorite = false;
        }

        public Customer(CustomerId id, NewCustomerData newCustomerData)
        {
            Id = id;
            Name = newCustomerData.Name;
            Street = newCustomerData.Street;
            ZipCode = newCustomerData.ZipCode;
            City = newCustomerData.City;
            Country = newCustomerData.Country;
            PictureUrl = String.Empty;
        }

        public Result<Customer> ExecuteMarkAsFavorite()
        {
            if (CanExecuteMarkAsFavorite())
            {
                IsFavorite = true;
                return Result<Customer>.Ok(this);
            }

            return Result<Customer>.Error("Already marked as favorite");
        }
        public bool CanExecuteMarkAsFavorite()
        {
            return !IsFavorite;
        }
        public Result<Customer> ExecuteUnMarkAsFavorite()
        {
            if (CanExecuteUnMarkAsFavorite())
            {
                IsFavorite = false;
                return Result<Customer>.Ok(this);
            }

            return Result<Customer>.Error("Already unmarked as favorite");
        }
        public bool CanExecuteUnMarkAsFavorite()
        {
            return IsFavorite;
        }
        public Result<Unit> Move(Address newAddress)
        {
            Street = newAddress.Street;
            ZipCode = newAddress.ZipCode;
            City = newAddress.City;
            Country = newAddress.Country;
            return Result<Unit>.Ok(Unit.Instance);
        }
    }

    public record CustomerId : IdBase
    {
        public CustomerId(Guid value) : base(value)
        {
        }
    }
}