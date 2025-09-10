

using BasketApi.Data;

namespace BasketApi.Basket.DeleteBasket
{
    public record DeleteBasketCommand(string UserName
        ) : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool Success);

    public class DeleteBasketValidator
        : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketValidator()
        {
            RuleFor(x => x.UserName).NotEmpty()
                .WithMessage("Username can not be empty");
        }
    }
    public class DeleteBasketHandler
        (IBasketRepository repository)
        : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            await repository.DeleteBasket(request.UserName, cancellationToken);

            return await Task.FromResult(new DeleteBasketResult(true));
        }
    }
}
