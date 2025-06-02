using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace TestTaskSMS.WPF.ViewModel;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<CustomEnvironment> Variables { get; set; } = new();
    public ICommand SaveVariableCommand { get; }

    public MainViewModel()
    {
        GetVariables();
        SaveVariableCommand = new RelayCommand(async () => await SaveVariablesAsync());

    }

    private void GetVariables()
    {
        var environments = App.Configuration.GetSection("Environments").Get<string[]>() ?? Array.Empty<string>(); ;
        foreach (var environment in environments)
        {
            var value = Environment.GetEnvironmentVariable(environment, EnvironmentVariableTarget.User) ?? string.Empty;
            Variables.Add(new CustomEnvironment { Name = environment, Value = value });
        }
    }

    private async Task SaveVariablesAsync()
    {

        await Task.Run(() =>
        {
            foreach (var variable in Variables)
            {
                var oldValue = Environment.GetEnvironmentVariable(variable.Name, EnvironmentVariableTarget.User) ?? null;

                Environment.SetEnvironmentVariable(variable.Name, variable.Value,EnvironmentVariableTarget.User);

                Log.Information($"Значение переменной {variable.Name} изменено с  '{oldValue}' на '{variable.Value}'");
            }
        });

        MessageBox.Show("Переменные сохранены","Success");

    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }

}
