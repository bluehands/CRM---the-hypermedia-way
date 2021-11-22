using CRM.Application;
using CRM.Domain;
using FunicularSwitch;
using Newtonsoft.Json;

#pragma warning disable CS1998

namespace CRM.InMemoryRepository
{
    public class VolatileCustomerRepository : ICustomerRepository
    {
        private readonly Dictionary<CustomerId, Customer> m_Customers = new();
        public VolatileCustomerRepository()
        {
            var url = "https://randomuser.me/api/?results=20";
            var httpClient = new HttpClient();
            var usersSerialized = httpClient.GetStringAsync(url).Result;
            var users = JsonConvert.DeserializeObject<Root>(usersSerialized);
            foreach (var apiResult in users.Results!)
            {
                var customer = new Customer(
                    new CustomerId(Guid.NewGuid()),
                    $"{apiResult.Name?.First} {apiResult.Name?.Last}",
                    $"{apiResult.Location?.Street?.Name} {apiResult.Location?.Street?.Number}",
                    $"{apiResult.Location?.Postcode}",
                    $"{apiResult.Location?.City}",
                    $"{apiResult.Location?.Country}",
                    apiResult.Picture?.Large!
                );

                m_Customers.Add(customer.Id, customer);
            }
        }

        public async Task<Result<IEnumerable<Customer>>> GetAll()
        {
            return new List<Customer>(m_Customers.Values);
        }

        public async Task<Result<IEnumerable<Customer>>> GetAllFavorites()
        {
            return new List<Customer>(m_Customers.Values.Where(c=>c.IsFavorite));
        }

        public async Task<Result<Customer>> GetById(CustomerId customerId)
        {
            if (m_Customers.TryGetValue(customerId, out Customer? customer))
            {
                return Result.Ok(customer);
            }
            return Result.Error<Customer>($"Customer with id {customerId.Value} not found");
        }

        public async Task<Result<Unit>> Update(Customer customer)
        {
            m_Customers[customer.Id] = customer;
            return Result.Ok(Unit.Instance);
        }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Name
    {
        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("first")]
        public string? First { get; set; }

        [JsonProperty("last")]
        public string? Last { get; set; }
    }

    public class Street
    {
        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }

    public class Coordinates
    {
        [JsonProperty("latitude")]
        public string? Latitude { get; set; }

        [JsonProperty("longitude")]
        public string? Longitude { get; set; }
    }

    public class Timezone
    {
        [JsonProperty("offset")]
        public string? Offset { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }
    }

    public class Location
    {
        [JsonProperty("street")]
        public Street? Street { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("state")]
        public string? State { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("postcode")]
        public object? Postcode { get; set; }

        [JsonProperty("coordinates")]
        public Coordinates? Coordinates { get; set; }

        [JsonProperty("timezone")]
        public Timezone? Timezone { get; set; }
    }

    public class Login
    {
        [JsonProperty("uuid")]
        public string? Uuid { get; set; }

        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonProperty("password")]
        public string? Password { get; set; }

        [JsonProperty("salt")]
        public string? Salt { get; set; }

        [JsonProperty("md5")]
        public string? Md5 { get; set; }

        [JsonProperty("sha1")]
        public string? Sha1 { get; set; }

        [JsonProperty("sha256")]
        public string? Sha256 { get; set; }
    }

    public class Dob
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }
    }

    public class Registered
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }
    }

    public class Id
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("value")]
        public string? Value { get; set; }
    }

    public class Picture
    {
        [JsonProperty("large")]
        public string? Large { get; set; }

        [JsonProperty("medium")]
        public string? Medium { get; set; }

        [JsonProperty("thumbnail")]
        public string? Thumbnail { get; set; }
    }

    public class ApiResult
    {
        [JsonProperty("gender")]
        public string? Gender { get; set; }

        [JsonProperty("name")]
        public Name? Name { get; set; }

        [JsonProperty("location")]
        public Location? Location { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("login")]
        public Login? Login { get; set; }

        [JsonProperty("dob")]
        public Dob? Dob { get; set; }

        [JsonProperty("registered")]
        public Registered? Registered { get; set; }

        [JsonProperty("phone")]
        public string? Phone { get; set; }

        [JsonProperty("cell")]
        public string? Cell { get; set; }

        [JsonProperty("id")]
        public Id? Id { get; set; }

        [JsonProperty("picture")]
        public Picture? Picture { get; set; }

        [JsonProperty("nat")]
        public string? Nat { get; set; }
    }

    public class Info
    {
        [JsonProperty("seed")]
        public string? Seed { get; set; }

        [JsonProperty("results")]
        public int Results { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("version")]
        public string? Version { get; set; }
    }

    public class Root
    {
        [JsonProperty("results")]
        public List<ApiResult>? Results { get; set; }

        [JsonProperty("info")]
        public Info? Info { get; set; }
    }


}