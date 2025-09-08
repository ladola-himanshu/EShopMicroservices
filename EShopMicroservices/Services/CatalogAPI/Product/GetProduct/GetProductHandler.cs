using BuilingBlocks.CQRS;
using Marten;
using Marten.Pagination;
using dto = CatalogAPI.Modal;

namespace CatalogAPI.Product.GetProduct
{
    public record GetProductsQuery(int? pageNumber = 1,
        int? pageSize = 10) 
        : IQuery<GetProductResult>;
    
    public record GetProductResult(IEnumerable<dto.Product> Products);

    public class GetProductHandler
        (IDocumentSession session)
        : IQueryHandler<GetProductsQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
//            var products = await session.Query<dto.Product>().ToListAsync(cancellationToken);
            var products = await session.Query<dto.Product>()
                .ToPagedListAsync(request.pageNumber ?? 1,
                request.pageSize ?? 3, cancellationToken);

            return new GetProductResult(products);
        }
    }
}
