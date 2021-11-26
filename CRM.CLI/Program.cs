﻿// See https://aka.ms/new-console-template for more information

using System.Net.Http.Headers;
using Bluehands.Hypermedia.Client;
using Bluehands.Hypermedia.Client.Authentication;
using Bluehands.Hypermedia.Client.Extensions.SystemNetHttp;
using Bluehands.Hypermedia.Client.Extensions.SystemTextJson;
using CRM.Server.Controllers;

namespace CRM.CLI;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var entryPointUrl = new Uri("https://localhost:5001/api/entrypoint");
        var sirenClient = CreateSirenClient(entryPointUrl);

        throw new NotImplementedException();
        Console.ReadLine();

    }

    private static HypermediaClient<EntryPointHco> CreateSirenClient(Uri entryPoint)
    {
        return new HypermediaClientBuilder()
            .ConfigureObjectRegister(register =>
                {
                    register.Register<EntryPointHco>();
                    register.Register<CustomersRootHco>();
                    register.Register<CampaignsRootHco>();
                    register.Register<CustomerQueryResultHco>();
                    register.Register<CustomerHco>();
                })
            .WithSingleSystemTextJsonObjectParameterSerializer()
            .WithHttpHypermediaResolver(resolver =>
                {
                    resolver.SetCredentials(new UsernamePasswordCredentials("User", "Password"));
                    resolver.SetCustomDefaultHeaders(headers => headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en", 1.0)));
                })
            .WithSystemTextJsonStringParser()
            .WithSystemTextJsonProblemReader()
            .WithSirenHypermediaReader()
            .CreateHypermediaClient<EntryPointHco>(entryPoint);
    }

}
