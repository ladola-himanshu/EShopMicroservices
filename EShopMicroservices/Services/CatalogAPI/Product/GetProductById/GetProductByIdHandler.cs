using BuilingBlocks.CQRS;
using Carter;
using Marten;
using Marten.Linq.QueryHandlers;
using dto = CatalogAPI.Modal;

namespace CatalogAPI.Product.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(dto.Product Product);
    public class GetProductByIdHandler
        (IDocumentSession session, ILogger<GetProductByIdHandler> logger)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetProductByIdQuery for Id: {Id}", request.Id);
            var product = session.Query<dto.Product>().FirstOrDefault(x => x.Id == request.Id);
            if (product is null)
            {
                logger.LogWarning("Product with Id: {Id} not found", request.Id);
                throw new ProductNotFoundException($"Product with Id: {request.Id} not found");
            }
            return await Task.FromResult(new GetProductByIdResult(product));
        }
    }
}
