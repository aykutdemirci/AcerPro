using System.Threading.Tasks;

namespace AcerPro.Business.Services.NotificationService
{
    public interface INotificationService
    {
       void SendNotification(string message);
    }
}
