using BuilingBlocks.CQRS;
using Marten;
using dto = CatalogAPI.Modal;

namespace CatalogAPI.Product.GetProduct
{
    public record GetProductsQuery : IQuery<GetProductResult>;
    public record GetProductResult(IEnumerable<dto.Product> Products);

    public class GetProductHandler
        (IDocumentSession session)
        : IQueryHandler<GetProductsQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await session.Query<dto.Product>().ToListAsync(cancellationToken);
            return new GetProductResult(products);
        }
    }
}
