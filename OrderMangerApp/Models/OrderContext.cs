using Microsoft.EntityFrameworkCore;

namespace OrderManagerApp.Models;

public class OrderContext() : DbContext
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite("Data Source=orders.db");
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