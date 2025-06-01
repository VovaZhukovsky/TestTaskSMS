using Microsoft.EntityFrameworkCore;
using TestTaskSMS.SMSHttpClient.Model;

namespace TestTaskSMS.ConsoleApp;
public class ApplicationContext : DbContext
{
    public DbSet<Dish> Dishes => Set<Dish>();
    private string _dbConnectionString;
    public ApplicationContext(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
        Database.EnsureCreated();
    }
 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_dbConnectionString);
    }
}