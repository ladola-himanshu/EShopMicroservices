using Carter;

namespace BasketApi.Basket.GetBasket
{
    public record GetBasketRequest
        (
        string UserName
        );
    public record GetBasketResponse(ShoppingCart cart);
    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string username,
                ISender sender) =>
                {
                    var result = await sender.Send(new GetBasketQuery(username));
                    //var response = result.Adapt<GetBasketResponse>();
                    return Results.Ok(result);
                })
                .WithName("GetBasket")
                .Produces<GetBasketResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get Basket")
                .WithDescription("Get Basket for a specific user");
        }
    }
}
