using BuilingBlocks.CQRS;
using FluentValidation;
using Marten;
using System.Windows.Input;
using dto = CatalogAPI.Modal;

namespace CatalogAPI.Product.UpdateProduct
{
    public record UpdateProductCommand(Guid Id,
        string Name, string Description, 
        decimal Price, List<string> category,
        string ImageUrl)
        : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool Success);
    
    public class UpdateProductValidator : 
        AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                .WithMessage("Id is required for updating product");

            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Name is required for product")
                .Length(2, 100).WithMessage("Name must be between 2 to 100 characters");

            RuleFor(x => x.Price).GreaterThan(0)
                .WithMessage("Price can not be 0 or less then 0");

        }
    }

    public class UpdateProductHandler
        (IDocumentSession session,
        ILogger<UpdateProductHandler> logger)
        : ICommandHandler<UpdateProductCommand, 
            UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(
            UpdateProductCommand request, 
            CancellationToken cancellationToken)
        {
            dto.Product product = await session.LoadAsync<dto.Product>(request.Id);
            if (product == null)
            {
                logger.LogWarning("Product with Id: {ProductId} not found", request.Id);
                return await Task.FromResult(new UpdateProductResult(false));
            }
            else
            {
                var productId = product.Id;
                product.Name = request.Name;
                product.Description = request.Description;
                product.Price = request.Price;
                product.Category = request.category;
                product.ImageUrl = request.ImageUrl;
                session.Update(product);
                await session.SaveChangesAsync();
                logger.LogInformation("Product with Id: {ProductId} updated successfully", productId);
            }
            return await Task.FromResult(new UpdateProductResult(true));
        }
    }
}
