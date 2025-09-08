using BuilingBlocks.CQRS;
using FluentValidation;
using Marten;
using dto = CatalogAPI.Modal;

namespace CatalogAPI.Product.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(List<dto.Product> Products);
    
    public class GetProductByCategoryValidator :
        AbstractValidator<GetProductByCategoryQuery>
    {
        public GetProductByCategoryValidator()
        {
            RuleFor(x => x.Category).NotEmpty()
                .WithMessage("Category must be provided to retrieve the products");
        }
    }

    public class GetProductByCategoryHandler
        (IDocumentSession session,
            ILogger<GetProductByCategoryHandler> logger)
        : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = session.Query<dto.Product>().ToList();
            var filteredProducts = products
                .Where(x => x.Category.Contains(request.Category, StringComparer.OrdinalIgnoreCase))
                .ToList();
            return await Task.FromResult(new GetProductByCategoryResult(filteredProducts));
        }
    }
}
