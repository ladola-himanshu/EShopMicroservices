using Carter;
using Mapster;
using MediatR;

namespace CatalogAPI.Product.DeleteProduct
{
    public record DeleteProductResponse(bool Success);
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductCommand(id));
                var response = result.Adapt<DeleteProductResponse>();
                if(response.Success)
                {
                    return Results.Ok(response);
                }
                else
                {
                    return Results.NotFound();
                }
            })
            .WithTags("Products")
            .WithName("DeleteProduct")
            .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
