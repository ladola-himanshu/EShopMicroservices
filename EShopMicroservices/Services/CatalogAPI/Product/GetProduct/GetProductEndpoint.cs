using BuilingBlocks.CQRS;
using Carter;
using Mapster;
using Marten;
using MediatR;
using dto =  CatalogAPI.Modal;

namespace CatalogAPI.Product.GetProduct
{
    public record GetProductsResponse(IEnumerable<dto.Product> Products);
    
    public class GetProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", 
                async ([AsParameters] GetProductsQuery request, ISender sender
                ) =>
            {
                //var query = request.Adapt<GetProductsQuery>();
                var query = request;
                //var query = new GetProductsQuery();
                var result = await sender.Send(query);
                var response = result.Adapt<GetProductsResponse>();

                return Results.Ok(response);
            })
            .WithTags("Products")
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
