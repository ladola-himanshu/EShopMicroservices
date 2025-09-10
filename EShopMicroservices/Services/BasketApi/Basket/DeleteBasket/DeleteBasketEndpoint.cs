
namespace BasketApi.Basket.DeleteBasket
{
    public record DeleteBasketRequest(string userName);
    public record DeleteBasketResponse(bool Success);
    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket",
                async (string userName, ISender sender) =>
                {
                    var result = await sender.Send(new DeleteBasketCommand(userName));

                    var response = result.Adapt<DeleteBasketResponse>();

                    return Results.Ok(response);
                })
                .WithName("DeleteBasket")
                .WithDescription("Delete basket for given username")
                .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);
            
        }
    }
}
