using Microsoft.EntityFrameworkCore;

namespace OrderManagerApp.Models;

public class OrderContext() : DbContext
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite("Data Source=orders.db");

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
// try
//         {
//             string appDataPath =  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

//             string myAppFolder = Path.Combine(appDataPath, Assembly.GetEntryAssembly()?.GetName().Name);
//             if (!Directory.Exists(myAppFolder))
//             {
//                 Directory.CreateDirectory(myAppFolder);
//                 Console.WriteLine($"Create folder: {myAppFolder}");
//             }
//             string dbPath = Path.Combine(myAppFolder, "orders.db");

//             optionsBuilder.UseSqlite($"Data Source={dbPath}");
//         }
//         catch (Exception err)
//         {
//             Console.WriteLine(err);
//         }