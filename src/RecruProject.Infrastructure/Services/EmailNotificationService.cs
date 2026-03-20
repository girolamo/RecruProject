using RecruProject.Core.Services;

namespace RecruProject.Infrastructure.Services;

public class EmailNotificationService : INotificationService
{
    public async Task Send(string message)
    {
        // Imagining that we are connecting to SMTP server and sending message VIA email 
        await Task.Delay(100);
    }
}