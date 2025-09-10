


using BasketApi.Data;

namespace BasketApi.Basket.StoreBasket
{
    public record StoreBasketCommand
        (
        //string UserName,
        ShoppingCart Basket
        ) : ICommand<StoreBasketResult>;

    public record StoreBasketResult(string userName);
    
    public class StoreBasketValidator
        : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketValidator()
        {
            RuleFor(x => x.Basket.UserName).NotEmpty()
                .WithMessage("Username can not be empty");
            RuleFor(x => x.Basket).NotNull()
                .WithMessage("Basket can not be null");
        }
    }
    public class StoreBasketHandler
        (IBasketRepository repository)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(
            StoreBasketCommand request,
            CancellationToken cancellationToken)
        {
            ShoppingCart cart = request.Basket;
            await repository.StoreBasket(cart, cancellationToken);
            return await Task.FromResult(new StoreBasketResult(cart.UserName));
        }
    }
}
