using System.Threading.Tasks;
using MyCompany.Crm.Sales.Orders;
using MyCompany.Crm.Sales.Products;
using MyCompany.Crm.TechnicalStuff;
using MyCompany.Crm.TechnicalStuff.Metadata;
using MyCompany.Crm.TechnicalStuff.Metadata.DDD;
using MyCompany.Crm.TechnicalStuff.UseCases;

namespace MyCompany.Crm.Sales.Wholesale.AddToOrder
{
    [Stateless]
    [DddAppService]
    public class AddToOrderHandler : CommandHandler<AddToOrder, AddedToOrder>
    {
        private readonly OrderRepository _orders;

        public AddToOrderHandler(OrderRepository orders) => _orders = orders;

        public async Task<AddedToOrder> Handle(AddToOrder command)
        {
            var (orderId, productAmount) = CreateDomainModelFrom(command);
            var order = await _orders.GetBy(orderId);
            order.Add(productAmount);
            await _orders.Save(order);
            return CreateEventFrom(orderId, productAmount);
        }

        private static (OrderId, ProductAmount) CreateDomainModelFrom(AddToOrder command) => (
            OrderId.From(command.OrderId),
            ProductAmount.Of(
                ProductId.From(command.ProductId), 
                command.Amount, 
                command.UnitCode.ToDomainModel<AmountUnit>()));
        
        private static AddedToOrder CreateEventFrom(OrderId orderId, ProductAmount productAmount) => 
            new AddedToOrder(orderId.Value, 
                productAmount.ProductId.Value, 
                productAmount.Amount.Value, 
                productAmount.Amount.Unit.ToCode());
    }
}