using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OrderManagerApp.Models;

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

}