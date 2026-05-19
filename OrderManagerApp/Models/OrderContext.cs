using Microsoft.EntityFrameworkCore;

namespace OrderManagerApp.Models;

public class OrderContext() : DbContext
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string project_name = "OrderManager";

        string db_path = Path.Combine(path, project_name, "orders.db");

        optionsBuilder.UseSqlite($"Data Source={db_path}");
    }

    public static IAsyncEnumerable<Order> GetOpenOrdersAsync(OrderContext ctx, ShippingMethod? method = null, bool descending = false)
    {
        var openOrders = ctx.Orders.AsNoTracking().Where(NotDeletedFilter);
        if (method is not null)
            openOrders = openOrders.Where(order => order.Method == method);

        openOrders = descending ?
        openOrders.OrderByDescending(OrderNumberOrderingFunc):
        openOrders.OrderBy(OrderNumberOrderingFunc);

        return openOrders.ToAsyncEnumerable();
    }

    public static IAsyncEnumerable<Order> GetClosedOrdersAsync(OrderContext ctx, ShippingMethod? method = null, bool descending = false)
    {
        var closedOrders = ctx.Orders.AsNoTracking().Where(IsDeletedFilter);

        if (method is not null)
            closedOrders = closedOrders.Where(order => order.Method == method);

        closedOrders = descending ?
        closedOrders.OrderByDescending(OrderNumberOrderingFunc):
        closedOrders.OrderBy(OrderNumberOrderingFunc);

        return closedOrders.ToAsyncEnumerable();
    }

    public static IAsyncEnumerable<Order> GetAllOrdersAsync(OrderContext ctx, bool descending = false)
    {
        var order = ctx.Orders.AsNoTracking();

        var orderedOrders = descending ?
        order.OrderByDescending(OrderNumberOrderingFunc):
        order.OrderBy(OrderNumberOrderingFunc);

        return orderedOrders.ToAsyncEnumerable();
    }

    private static bool NotDeletedFilter(Order order) => !order.isDeleted;
    private static bool IsDeletedFilter(Order order) => order.isDeleted;
    private static uint OrderNumberOrderingFunc(Order order) => order.OrderNumber;
}