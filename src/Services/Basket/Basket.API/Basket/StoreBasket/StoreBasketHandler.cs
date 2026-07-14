using Discount.Grpc;
using static Discount.Grpc.DiscountProtoService;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);
    public class StoreBasketCommandValidatore : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidatore()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Shopping cart is required");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Cart.Items).NotNull().WithMessage("Basket items are required");
        }
    }

    internal class StoreBasketCommandHandler(IBasketRepository repository, DiscountProtoServiceClient discountProto)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand commannd, CancellationToken cancellationToken)
        {
            await DeductDiscount(commannd.Cart, cancellationToken);
            await repository.StoreBasket(commannd.Cart, cancellationToken);
            return new StoreBasketResult(commannd.Cart.UserName);
        }

        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                item.Price -= coupon.Amount;
            }   
        }
    }
}
