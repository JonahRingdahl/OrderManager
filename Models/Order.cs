using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OrderManager.Models;

public enum ShippingMethod
{
    CPUP,
    BACKORDER,
    SHIPPING
}

public class Order (
    bool isPulled,
    uint orderNumber,
    string poNumber,
    ShippingMethod method,
    DateTime orderEntered
) : IComparable<Order>
{
    [Key]
    public uint OrderNumber { get; set; } = orderNumber;
    public string PoNumber { get; set; } = poNumber;
    public ShippingMethod Method { get; set; } = method;
    public DateTime OrderEntered { get; set; } = orderEntered;
    public bool IsPulled { get; set; } = isPulled;
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
    public bool isDeleted { get; set; } = false;

    public int CompareTo(Order? obj) => OrderNumber.CompareTo(obj?.OrderNumber);

    public void UndeleteOrder()
    {
        isDeleted = false;
        UpdatedDate = DateTime.Now;

    }

    public string DisplayOrder() =>
        $"{OrderNumber} / {PoNumber} / Shipping: {Method} / Pulled? {IsPulled} / DELETED: {isDeleted}\n\n";

    public static IAsyncEnumerable<Order> GetOpenOrdersAsync(OrderContext ctx, bool descending = false)
    {
        var openOrders = ctx.Orders.AsNoTracking().Where(NotDeletedFunc);

        openOrders = descending ?
        openOrders.OrderByDescending(OrderNumberOrderingFunc):
        openOrders.OrderBy(OrderNumberOrderingFunc);

        return openOrders.ToAsyncEnumerable();
    }

    public static IAsyncEnumerable<Order> GetClosedOrdersAsync(OrderContext ctx, bool descending = false)
    {
        var closedOrders = ctx.Orders.AsNoTracking().Where(IsDeletedFunc);

        closedOrders = descending ?
        closedOrders.OrderByDescending(OrderNumberOrderingFunc):
        closedOrders.OrderBy(OrderNumberOrderingFunc);

        return closedOrders.ToAsyncEnumerable();
    }

    public static IAsyncEnumerable<Order> GetAllOrdersAsync(OrderContext ctx, bool descending = false)
    {
        var order= ctx.Orders.AsNoTracking();

        var orderedOrders = descending ?
        order.OrderByDescending(OrderNumberOrderingFunc):
        order.OrderBy(OrderNumberOrderingFunc);

        return orderedOrders.ToAsyncEnumerable();
    }

    private static bool NotDeletedFunc(Order order) => !order.isDeleted;
    private static bool IsDeletedFunc(Order order) => order.isDeleted;
    private static uint OrderNumberOrderingFunc(Order order) => order.OrderNumber;


}