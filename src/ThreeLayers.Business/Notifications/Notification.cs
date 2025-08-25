namespace ThreeLayers.Business.Notifications;

public class Notification
{
    public string Message { get; private set; }
    public NotificationType Type { get; private set; }

    public Notification(string message, NotificationType type = NotificationType.Validation)
    {
        Message = message;
        Type = type;
    }
}