using Carter;
using MediatR;
using dto = CatalogAPI.Modal;
namespace CatalogAPI.Product.GetProductByCategory
{
    public record GetProductByCategoryResponse(List<dto.Product> Products);
    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", 
                async (string category, 
                ISender queryProcessor) =>
            {
                var result = await queryProcessor.Send(new GetProductByCategoryQuery(category));
                return Results.Ok(new GetProductByCategoryResponse(result.Products));
            })
            .WithName("GetProductByCategory")
            .WithTags("CatalogAPI")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK);
        }
    }
}
