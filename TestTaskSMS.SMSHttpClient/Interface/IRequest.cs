namespace TestTaskSMS.SMSHttpClient.Interface;
public interface IRequest<T>
{
    public string Command { get; set; }
    public T CommandParameters { get; set; }
}