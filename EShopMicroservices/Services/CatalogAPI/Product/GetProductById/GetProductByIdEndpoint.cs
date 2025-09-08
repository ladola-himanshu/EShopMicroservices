using Carter;
using MediatR;
using dto = CatalogAPI.Modal;

namespace CatalogAPI.Product.GetProductById
{
    public record GetProductByIdResponse(dto.Product product);
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id:guid}", async (Guid id, ISender queryProcessor) =>
            {
                var result = await queryProcessor.Send(new GetProductByIdQuery(id));
                return Results.Ok(new GetProductByIdResponse(result.Product));
            })
            .WithName("GetProductById")
            .WithTags("CatalogAPI")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
