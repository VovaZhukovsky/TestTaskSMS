using TestTaskSMS.CommonLibrary.Interface;

namespace TestTaskSMS.CommonLibrary.Model;

public class GetMenuRequest : IRequest<Price>
{
    public string Command { get; set; }
    public Price CommandParameters { get; set; } = new();
}

public class Price
{
    public bool WithPrice { get; set; } = true;
}

public class SendOrderRequest : IRequest<OrderParams>
{
    public string Command { get; set; }
    public OrderParams CommandParameters { get; set; } = new();
}

public class OrderParams
{
    public string OrderId { get; set; }
    public List<Order> MenuItems { get; set; } = new();
}

