

using BasketApi.Basket.DeleteBasket;
using BasketApi.Basket.GetBasket;
using BasketApi.Basket.StoreBasket;
using BasketApi.Data;
using Microsoft.CodeAnalysis.FlowAnalysis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter(null, cfg =>
    {
        //TODO: Register all carter modules in the assembly
        Type[] carterTypes = {
            typeof(GetBasketEndpoint),
            typeof(DeleteBasketEndpoint),
            typeof(StoreBasketEndpoint)
        };
        cfg.WithModules(carterTypes);
    }  
);
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});

builder.Services.AddMarten( opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("DefaultConnection")!);
    opt.Schema.For<BasketApi.Modal.ShoppingCart>().Identity(x => x.UserName);

}).UseLightweightSessions();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.MapGet("/basket", () => "Basket API is running");
app.MapCarter();

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

app.Run();

