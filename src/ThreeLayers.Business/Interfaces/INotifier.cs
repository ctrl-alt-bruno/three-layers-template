using ThreeLayers.Business.Notifications;

namespace ThreeLayers.Business.Interfaces;

public interface INotifier
{
    bool HasNotification();
    List<Notification> GetNotifications();
    void Handle(Notification notification);
}