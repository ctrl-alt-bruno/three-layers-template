using ThreeLayers.Business.Notifications;

namespace ThreeLayers.Business.Interfaces;

public interface INotifier
{
    bool HasNotification();
    bool HasNotificationOfType(NotificationType type);
    List<Notification> GetNotifications();
    List<Notification> GetNotificationsByType(NotificationType type);
    void Handle(Notification notification);
    void Clear();
}