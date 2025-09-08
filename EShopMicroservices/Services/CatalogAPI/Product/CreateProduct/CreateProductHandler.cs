using BuilingBlocks.CQRS;
using CatalogAPI.Modal;
using FluentValidation;
using Marten;

namespace CatalogAPI.Product.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, decimal Price, List<string> Category, string ImageUrl)
        : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid ProductId);
    
    public class CreateProductCommandValidator : 
        AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Product description is required.")
                .MaximumLength(1000).WithMessage("Product description cannot exceed 1000 characters.");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Product price must be greater than zero.");
            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("At least one category is required.");
            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Image URL is required.")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Image URL must be a valid URL.");
        }
    }

    public class CreateProductHandler
        (IDocumentSession session, ILogger<CreateProductHandler> logger)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Here you would typically add code to save the product to a database.
            // For this example, we'll just simulate creating a product and returning its ID.
            //var result = await validator.ValidateAsync(request, cancellationToken);
            //var errors = result.Errors;
            //if (errors.Any())
            //{
            //    throw new ValidationException(errors);
            //}
            var newProductId = Guid.NewGuid(); // Simulate generating a new product ID.
            // Change the reference from 'Product' (namespace) to the correct type, likely 'CatalogAPI.Modal.Product'
            logger.LogInformation("New product id {0} ", newProductId);

            var product = new CatalogAPI.Modal.Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Category = request.Category,
                ImageUrl = request.ImageUrl
            };

            //TODO : Save the product to the database.
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            // Return the result with the new product ID.
            return await Task.FromResult(new CreateProductResult(product.Id));
        }
    }
}
