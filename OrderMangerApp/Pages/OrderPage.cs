using OrderManagerApp.Models;

namespace OrderManagerApp.Pages;

partial class OrderPage : Form
{
    public EventHandler<Order>? OrderSaved;
    private Form prevForm;
    Order? order;

    // bool isPulled,
    //     uint orderNumber,
    //     string poNumber,
    //     ShippingMethod method,
    //     DateTime orderEntered

    public OrderPage(Order? order, Form form)
    {
        InitializeComponent();

        if (order is not null)
            LoadData(order);

        prevForm = form;
        shippingMethodBox.DropDownStyle = ComboBoxStyle.DropDownList;
        shippingMethodBox.Items.AddRange(
            [ShippingMethod.CPUP, ShippingMethod.BACKORDER, ShippingMethod.SHIPPING]
        );
    }

    private void LoadData(Order o)
    {
        order = o;
        orderNumBox.Text = order.OrderNumber.ToString();
        poNumBox.Text = order.PoNumber;
        pulledCheckBox.Checked = order.IsPulled;
        switch (order.Method)
        {
            case ShippingMethod.CPUP:
                shippingMethodBox.SelectedValue = 0;
                break;

            case ShippingMethod.BACKORDER:
                shippingMethodBox.SelectedValue = 1;
                break;
            case ShippingMethod.SHIPPING:
                shippingMethodBox.SelectedValue = 1;
                break;
            default:
                break;
        }
    }

    private bool ValidateData()
    {
        var success = uint.TryParse(orderNumBox.Text, out uint on);

        if (!success)
            return false;

        if (string.IsNullOrWhiteSpace(poNumBox.Text))
            return false;

        return true;
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        prevForm.Show();
        Close();
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (!ValidateData())
        {
            MessageBox.Show("Failed to parse Input", "Bad input", MessageBoxButtons.OK);
            return;
        }

        var context = new OrderContext();
        var on = uint.Parse(orderNumBox.Text);

        var success = Enum.TryParse(shippingMethodBox.Text, out ShippingMethod method);
        if (!success)
        {
            MessageBox.Show("Failed to parse Shipping Method", "Bad Method", MessageBoxButtons.OK);
        }

        if (order is null)
        {
            if (context.Orders.FirstOrDefault(o => o.OrderNumber == on) is not null)
            {
                MessageBox.Show(
                    "Order has already been created",
                    "Order Exists",
                    MessageBoxButtons.OK
                );
                return;
            }
            order = new Order(pulledCheckBox.Checked, on, poNumBox.Text, method, DateTime.Now)
            {
                UpdatedDate = DateTime.Now
            };
        }
        else
        {
            order.PoNumber = poNumBox.Text;
            order.IsPulled = pulledCheckBox.Checked;
            order.Method = method;
            order.UpdatedDate = DateTime.Now;
        }

        OrderSaved?.Invoke(this, order);
        prevForm.Show();
        Close();
    }
}
