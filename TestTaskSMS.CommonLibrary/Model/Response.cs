namespace TestTaskSMS.CommonLibrary.Model;

public class BaseResponse
{
    public string Command { get; set; }
    public bool Success { get; set; } = true;
    public string ErrorMessage { get; set; } = System.String.Empty;
}

public class SendOrderRespose : BaseResponse
{
}

public class GetMenuResponse : BaseResponse
{
    public Data Data { get; set; } = new();
}

public class Data
{
    public List<Dish> MenuItems { get; set; }
}
