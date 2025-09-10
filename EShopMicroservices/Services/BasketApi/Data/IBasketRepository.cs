namespace BasketApi.Data
{
    public interface IBasketRepository
    {
        Task<Modal.ShoppingCart?> GetBasket(
            string userName,
            CancellationToken cancellation = default);
        Task<Modal.ShoppingCart> StoreBasket(
            Modal.ShoppingCart basket,
            CancellationToken cancellation = default);
        Task<bool> DeleteBasket(string userName, 
            CancellationToken cancellation = default);
    }
}
