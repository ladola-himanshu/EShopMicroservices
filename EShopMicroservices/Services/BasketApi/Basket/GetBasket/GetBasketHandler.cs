using BasketApi.Data;
using BasketApi.Modal;
using BuilingBlocks.CQRS;
using MediatR;

namespace BasketApi.Basket.GetBasket
{
    public record GetBasketQuery
        (
        string UserName
        ) : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCart cart);
    public class GetBasketHandler
        (IBasketRepository repository)
        : IRequestHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(
            GetBasketQuery query,
            CancellationToken cancellationToken)
        {
            var basket = await repository.GetBasket(
                query.UserName,
                cancellationToken);

            if (basket != null)
            {
                return new GetBasketResult(basket);
            }
            
            return new GetBasketResult(new ShoppingCart());
        }
    }
}
