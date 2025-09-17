using ThreeLayers.Business.Interfaces;

namespace ThreeLayers.Business.Notifications;

public class Notifier : INotifier
{
    private readonly List<Notification> _notifications = new List<Notification>();

    public bool HasNotification()
    {
        return _notifications.Count != 0;
    }

    public bool HasNotificationOfType(NotificationType type)
    {
        return _notifications.Any(n => n.Type == type);
    }

    public List<Notification> GetNotifications()
    {
        return _notifications;
    }

    public List<Notification> GetNotificationsByType(NotificationType type)
    {
        return _notifications.Where(n => n.Type == type).ToList();
    }

    public void Handle(Notification notification)
    {
        _notifications.Add(notification);
    }

    public void Clear()
    {
        _notifications.Clear();
    }
}
