
namespace BasketApi.Basket.StoreBasket
{
    public record StoreBasketRequest(
        //string UserName,
        ShoppingCart Basket
        );
    public record StoreBasketResponse(string userName);
    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request,
                ISender send) =>
            {
                var command = request.Adapt<StoreBasketCommand>();
                var response = await send.Send(command);
                //var response = result.Adapt<StoreBasketResponse>();
                return Results.Created($"/basket/{response.userName}", response);
            })
                .WithSummary("Create basket using product items")
                .WithName("CreateBasket")
                .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
}
