using HealthCare.Business.Notifications;

namespace HealthCare.Business.Interfaces
{
    public interface INotifier
    {
        bool HasNotifications();
        List<Notification> GetNotificationAsync(Notification notification);
        List<Notification> GetNotificationAsync();
        void Handle(Notification notification);
    }
}