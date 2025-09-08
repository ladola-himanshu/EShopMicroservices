using BuilingBlocks.Behaviour;
using Carter;
using CatalogAPI.Data;
using CatalogAPI.Modal;
using CatalogAPI.Product.CreateProduct;
using CatalogAPI.Product.DeleteProduct;
using CatalogAPI.Product.GetProduct;
using CatalogAPI.Product.GetProductByCategory;
using CatalogAPI.Product.GetProductById;
using CatalogAPI.Product.UpdateProduct;
using FluentValidation;
using HealthChecks.UI.Client;
using Marten;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter(null, conf => {
    Type[] carterTypes = {
        typeof(CreateProductEndpoint),
        typeof(GetProductEndpoint),
        typeof(GetProductByIdEndpoint),
        typeof(GetProductByCategoryEndpoint),
        typeof(UpdateProductEndpoint),
        typeof(DeleteProductEndpoint)
    };
    conf.WithModules(carterTypes);
    });

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(assembly);
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
    });

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(options =>
{
    options.Connection(
        builder.Configuration
        .GetConnectionString("DefaultConnection")!);
    //options.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.All;
}).UseLightweightSessions();

if(builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.MapCarter();

app.MapGet("/catalogs", () => new[] { "Catalog1", "Catalog2", "Catalog3" });
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        var result = new
        {
            Title = exception?.Message,
            StatusCode = StatusCodes.Status500InternalServerError,
            Detail = exception?.StackTrace
        };

        await context.Response.WriteAsJsonAsync(result);
    });
});

app.UseHealthChecks("/health",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();

