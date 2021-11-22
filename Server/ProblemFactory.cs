using WebApi.HypermediaExtensions.ErrorHandling;
using WebApi.HypermediaExtensions.Exceptions;

namespace CRM.Server;

public class ProblemFactory 
{
    private const string ProblemTypeNamespace = "CRM";

    private bool AddExceptionInformation { get; }

    public ProblemFactory(IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            AddExceptionInformation = true;
        }
    }

    public ProblemJson NotFound()
    {
        return new ProblemJson
        {
            Title = "Route not found",
            Detail = "Requested a route which is not valid.",
            ProblemType = ProblemTypeNamespace + ".RouteNotFound",
            StatusCode = StatusCodes.Status404NotFound
        };
    }

    public ProblemJson EntityNotFound(string name, string id)
    {
        return new ProblemJson
        {
            Title = "Entity not found",
            Detail = $"Requested a resource which is not available. Type: '{name}' Id: '{id}'",
            ProblemType = ProblemTypeNamespace + ".EntityNotFound",
            StatusCode = StatusCodes.Status404NotFound
        };
    }

    public ProblemJson ServiceUnavailable()
    {

        return new ProblemJson
        {
            Title = "Service is not available",
            Detail = "Requestcould not be fullfilled because a required service is not available. Pease retry later.",
            ProblemType = ProblemTypeNamespace + ".ServiceUnavailable",
            StatusCode = StatusCodes.Status503ServiceUnavailable
        };
    }

    public ProblemJson BadParameters()
    {
        return new ProblemJson
        {
            Title = "Bad Parameters",
            Detail = "Can not execute action because the provided parameters are now acceptable. Review the parameter schema.",
            ProblemType = ProblemTypeNamespace + ".BadParameters",
            StatusCode = 400
        };
    }

    public ProblemJson OperationNotAvailable()
    {
        return new ProblemJson
        {
            Title = "Operation is not available",
            Detail = "Can not execute action because the operation can not be executed in the current state.",
            ProblemType = ProblemTypeNamespace + ".OperationNotAvailable",
            StatusCode = 403
        };
    }

    public ExceptionProblemJson Exception(HypermediaFormatterException hypermediaFormatterException)
    {
        return new ExceptionProblemJson(hypermediaFormatterException)
        {
            Title = "Hypermedia formatter error.",
            ProblemType = ProblemTypeNamespace + ".HyperrmediaFormatterError",
            StatusCode = StatusCodes.Status500InternalServerError,
            Detail = AddExceptionDetail(hypermediaFormatterException)
        };
    }

    public ExceptionProblemJson Exception(HypermediaException hypermediaException)
    {
        return new ExceptionProblemJson(hypermediaException)
        {
            Title = "Hypermedia error.",
            ProblemType = ProblemTypeNamespace + ".HyperrmediaError",
            StatusCode = StatusCodes.Status500InternalServerError,
            Detail = AddExceptionDetail(hypermediaException)
        };
    }

    public ExceptionProblemJson Exception(Exception exception)
    {
        return new ExceptionProblemJson(exception)
        {
            Title = "Sorry, something went wrong.",
            ProblemType = ProblemTypeNamespace + ".InternalError",
            StatusCode = StatusCodes.Status500InternalServerError,
            Detail = AddExceptionDetail(exception)
        };
    }
    public ProblemJson Exception(string errorMessage)
    {
        return new ProblemJson
        {
            Title = "Sorry, something went wrong.",
            ProblemType = ProblemTypeNamespace + ".InternalError",
            StatusCode = StatusCodes.Status500InternalServerError,
            Detail = AddExceptionDetail(errorMessage)
        };
    }
    private string AddExceptionDetail(Exception exception)
    {
       return AddExceptionDetail(exception.ToString());
    }
    private string AddExceptionDetail(string errorMessage)
    {
        if (AddExceptionInformation)
        {
            return errorMessage;
        }

        return string.Empty;
    }


}