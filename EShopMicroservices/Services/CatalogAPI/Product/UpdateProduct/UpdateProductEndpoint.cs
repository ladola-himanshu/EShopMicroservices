using Carter;
using Mapster;
using MediatR;

namespace CatalogAPI.Product.UpdateProduct
{
    /*
     * public record UpdateProductRequest(Guid Id,
        string Name, string Description,
        decimal Price, List<string> category,
        string ImageUrl);
    */
    public record UpdateProductResponse(bool Success);
    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products",
                async (UpdateProductCommand command, 
                ISender mediator) =>
            {
                var result = await mediator.Send(command);
                var response = result.Adapt<UpdateProductResponse>();
                return response.Success
                    ? Results.Ok(response)
                    : Results.NotFound();
            })
            .WithTags("Products")
            .WithName("UpdateProduct")
            .Produces<UpdateProductResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
