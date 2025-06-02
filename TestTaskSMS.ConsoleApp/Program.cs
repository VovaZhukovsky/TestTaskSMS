using TestTaskSMS.SMSHttpClient;
using TestTaskSMS.CommonLibrary.Model;
using static System.Console;
using Serilog;
using Microsoft.Extensions.Configuration;
using TestTaskSMS.ConsoleApp;

var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

SMSHttpClient smsHttpClient = new(configuration["HttpClientBaseAddress"],configuration["HttpClientUserName"],configuration["HttpClientUserName"]);

var getMenuResponse = await smsHttpClient.GetMenuAsync();

if (!getMenuResponse.Success)
{
    Log.Error(getMenuResponse.ErrorMessage);
    return;
}

using (ApplicationContext db = new ApplicationContext(configuration["DBConnectrionString"]))
{

    foreach (var item in getMenuResponse.Data.MenuItems)
    {
        db.Dishes.Add(item);
        Log.Information($"{item.Name}-{item.Article}-{item.Price}");
    }
    await db.SaveChangesAsync();
}

OrderParams orderParams = new() { OrderId = Guid.NewGuid().ToString()};
while (true)
{
    Write("Сделайте заказ: ");
    try
    {
        string dishes = ReadLine();
        foreach (var dish in dishes.Split(";"))
        {
            var dishInfo = dish.Split(":");
            Int32.Parse(dishInfo[0]);

            if (Int32.Parse(dishInfo[1]) <= 0)
                throw new Exception();

            orderParams.MenuItems.Add(new Order { Id = dishInfo[0].ToString(), Quantity = Convert.ToDouble(dishInfo[1]) });
        }
        break;
    }
    catch
    {
        Log.Error("Заказ неверный");
    }
}

var orderResponse = await smsHttpClient.SendOrderAsync(orderParams);

if (orderResponse.Success)
{
    Log.Information("УСПЕХ");
}
else
{
    Log.Error(orderResponse.ErrorMessage);
}




