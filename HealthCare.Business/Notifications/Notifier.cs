using HealthCare.Business.Interfaces;

namespace HealthCare.Business.Notifications
{
    class Notifier : INotifier
    {
        private List<Notification> _notifications;

        public Notifier()
        {
            _notifications = new List<Notification>();
        }

        public bool HasNotifications()
        {
            return _notifications.Any();
        }

        public List<Notification> GetNotificationAsync(Notification notification)
        {
            return _notifications;
        }

        public void Handle(Notification notification)
        {
            _notifications.Add(notification);
        }
    }
}