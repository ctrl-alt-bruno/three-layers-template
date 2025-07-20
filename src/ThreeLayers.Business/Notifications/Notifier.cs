using ThreeLayers.Business.Interfaces;

namespace ThreeLayers.Business.Notifications;

public class Notifier : INotifier
{
    private readonly List<Notification> _notifications = new List<Notification>();

    public bool HasNotification()
    {
        return _notifications.Count != 0;
    }

    public List<Notification> GetNotifications()
    {
        return _notifications;
    }

    public void Handle(Notification notification)
    {
        _notifications.Add(notification);
    }
}