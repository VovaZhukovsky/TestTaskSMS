using Microsoft.Extensions.Configuration;
using Serilog;
using System.Windows;

namespace TestTaskSMS.WPF;

public partial class App : Application
{
    public static IConfiguration Configuration { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {

        Configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .CreateLogger();

        base.OnStartup(e);
    }
}
