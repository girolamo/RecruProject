namespace RecruProject.Core.Services;

public interface INotificationService
{
    Task Send(string message);
}