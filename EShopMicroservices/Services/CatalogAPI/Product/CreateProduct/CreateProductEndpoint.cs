using BuilingBlocks.CQRS;
using Carter;
using Mapster;
using MediatR;

namespace CatalogAPI.Product.CreateProduct
{
    public record CreateProductRequest(string Name, string Description, decimal Price, List<string> Category, string ImageUrl);
    public record CreateProductResponse(Guid ProductId);
    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductRequest request,
                ISender send) =>
            {
                var command = request.Adapt<CreateProductCommand>();
                var result = await send.Send(command);
                var response = result.Adapt<CreateProductResponse>();
                return Results.Created($"/products/{response.ProductId}", response);

                //var command = new CreateProductCommand(request.Name, 
                //    request.Description, 
                //    request.Price,
                //    request.Category,
                //    request.ImageUrl);

                //var result = await handler.Handle(command, CancellationToken.None);
                //return Results.Created($"/products/{result.ProductId}", new CreateProductResponse(result.ProductId));
            })
            .WithName("CreateProduct")
            .WithTags("Products")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .WithSummary("Create a new product")
            .WithDescription("Creates a new product in the catalog and returns the product ID.");
        }
    }
}
