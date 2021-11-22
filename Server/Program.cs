using CRM.Application;
using CRM.InMemoryRepository;
using CRM.Server;

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
    options => options.OutputFormatters.Add(new CustomerImageFormatter())
    );


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
