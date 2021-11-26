using System.Reflection;
using CRM.Application;
using CRM.InMemoryRepository;
using CRM.Server;
using WebApi.HypermediaExtensions.WebApi.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<ICustomerRepository>(new VolatileCustomerRepository());
builder.Services.AddTransient<ProblemFactory>();
builder.Services.AddTransient<CustomerCommandHandler>();
builder.Services.AddTransient<CustomerMoveCommandHandler>();
builder.Services.AddTransient<FavoriteCustomersCommandHandler>();
builder.Services.AddLogging(o => o.AddConsole());
builder.Services.AddMvc(
    options =>
    {
        options.OutputFormatters.Add(new CustomerImageFormatter());
    }).
AddHypermediaExtensions(builder.Services,
    new HypermediaExtensionsOptions
    {
        ReturnDefaultRouteForUnknownHto = true // useful during development
    },
    Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(NewCustomerData))!
    );
builder.Services.AddCors(o =>
    o.AddPolicy("AllowAll", b =>
        {
            b.AllowAnyOrigin();
            b.AllowAnyHeader();
            b.AllowAnyMethod();
        }));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpLogging();
//app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();


app.Run();
