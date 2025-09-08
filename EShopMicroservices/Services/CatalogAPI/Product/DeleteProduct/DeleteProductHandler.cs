using BuilingBlocks.CQRS;
using FluentValidation;
using Marten;
using dto = CatalogAPI.Modal;

namespace CatalogAPI.Product.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool Success);
    
    public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                .WithMessage("Product id is must for delete operation");
        }
    }
    
    public class DeleteProductHandler
        (IDocumentSession session, ILogger<DeleteProductHandler> logger)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            session.Delete<dto.Product>(request.Id);
            session.SaveChangesAsync(cancellationToken);
            return Task.FromResult(new DeleteProductResult(true));
        }
    }
}
