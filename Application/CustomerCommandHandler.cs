using System.Collections;
using CRM.Domain;
using FunicularSwitch;

namespace CRM.Application
{

    public class CustomerCommandHandler
    {
        private readonly ICustomerRepository m_CustomerRepository;

        public CustomerCommandHandler(ICustomerRepository customerRepository)
        {
            m_CustomerRepository = customerRepository;
        }

        public async Task<Result<IEnumerable<Customer>>> GetAllCustomers()
        {
            return await m_CustomerRepository.GetAll();
        }
        public async Task<Result<IEnumerable<Customer>>> QueryCustomers(QueryParameter parameters)
        {
            return await m_CustomerRepository.GetAll().Bind(allCustomers =>
            {
                IEnumerable<Customer> queryResult = allCustomers;
                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    queryResult = queryResult.Where(c => c.Name.Contains(parameters.Name, StringComparison.InvariantCultureIgnoreCase));
                }

                if (!string.IsNullOrEmpty(parameters.Country))
                {
                    queryResult = queryResult.Where(c => c.Country.Contains(parameters.Country, StringComparison.InvariantCultureIgnoreCase));
                }

                return Result.Ok<IEnumerable<Customer>>(queryResult);
            });
        }

        public async Task<Result<Customer>> GetCustomerById(Guid id)
        {
            return await m_CustomerRepository.GetById(new CustomerId(id));
        }

        public async Task<Result<Customer>> AddCustomer(NewCustomerData newCustomerData)
        {
            var customer = new Customer(new CustomerId(Guid.NewGuid()), newCustomerData);
            await m_CustomerRepository.Update(customer);
            return Result.Ok<Customer>(customer);
        }
    }

    public record QueryParameter(string? Name, string? Country);

    public abstract class GetCustomerResult
    {
        public static readonly GetCustomerResult Ok = new Ok_();
        public static readonly GetCustomerResult NotFound = new NotFound_();
        public static readonly GetCustomerResult NotAuthorized = new NotAuthorized_();

        public class Ok_ : GetCustomerResult
        {
            public Ok_() : base(UnionCases.Ok)
            {
            }
        }

        public class NotFound_ : GetCustomerResult
        {
            public NotFound_() : base(UnionCases.NotFound)
            {
            }
        }

        public class NotAuthorized_ : GetCustomerResult
        {
            public NotAuthorized_() : base(UnionCases.NotAuthorized)
            {
            }
        }

        internal enum UnionCases
        {
            Ok,
            NotFound,
            NotAuthorized
        }

        internal UnionCases UnionCase { get; }
        GetCustomerResult(UnionCases unionCase) => UnionCase = unionCase;

        public override string ToString() => Enum.GetName(typeof(UnionCases), UnionCase) ?? UnionCase.ToString();
        bool Equals(GetCustomerResult other) => UnionCase == other.UnionCase;

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((GetCustomerResult)obj);
        }

        public override int GetHashCode() => (int)UnionCase;
    }

    public static class GetCustomerResultExtension
    {
        public static T Match<T>(this GetCustomerResult getCustomerResult, Func<GetCustomerResult.Ok_, T> ok, Func<GetCustomerResult.NotFound_, T> notFound, Func<GetCustomerResult.NotAuthorized_, T> notAuthorized)
        {
            switch (getCustomerResult.UnionCase)
            {
                case GetCustomerResult.UnionCases.Ok:
                    return ok((GetCustomerResult.Ok_)getCustomerResult);
                case GetCustomerResult.UnionCases.NotFound:
                    return notFound((GetCustomerResult.NotFound_)getCustomerResult);
                case GetCustomerResult.UnionCases.NotAuthorized:
                    return notAuthorized((GetCustomerResult.NotAuthorized_)getCustomerResult);
                default:
                    throw new ArgumentException($"Unknown type derived from GetCustomerResult: {getCustomerResult.GetType().Name}");
            }
        }

        public static async Task<T> Match<T>(this GetCustomerResult getCustomerResult, Func<GetCustomerResult.Ok_, Task<T>> ok, Func<GetCustomerResult.NotFound_, Task<T>> notFound, Func<GetCustomerResult.NotAuthorized_, Task<T>> notAuthorized)
        {
            switch (getCustomerResult.UnionCase)
            {
                case GetCustomerResult.UnionCases.Ok:
                    return await ok((GetCustomerResult.Ok_)getCustomerResult).ConfigureAwait(false);
                case GetCustomerResult.UnionCases.NotFound:
                    return await notFound((GetCustomerResult.NotFound_)getCustomerResult).ConfigureAwait(false);
                case GetCustomerResult.UnionCases.NotAuthorized:
                    return await notAuthorized((GetCustomerResult.NotAuthorized_)getCustomerResult).ConfigureAwait(false);
                default:
                    throw new ArgumentException($"Unknown type derived from GetCustomerResult: {getCustomerResult.GetType().Name}");
            }
        }

        public static async Task<T> Match<T>(this Task<GetCustomerResult> getCustomerResult, Func<GetCustomerResult.Ok_, T> ok, Func<GetCustomerResult.NotFound_, T> notFound, Func<GetCustomerResult.NotAuthorized_, T> notAuthorized) => (await getCustomerResult.ConfigureAwait(false)).Match(ok, notFound, notAuthorized);
        public static async Task<T> Match<T>(this Task<GetCustomerResult> getCustomerResult, Func<GetCustomerResult.Ok_, Task<T>> ok, Func<GetCustomerResult.NotFound_, Task<T>> notFound, Func<GetCustomerResult.NotAuthorized_, Task<T>> notAuthorized) => await (await getCustomerResult.ConfigureAwait(false)).Match(ok, notFound, notAuthorized).ConfigureAwait(false);
    }
}