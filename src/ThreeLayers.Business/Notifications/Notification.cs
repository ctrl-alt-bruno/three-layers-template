namespace ThreeLayers.Business.Notifications;

public class Notification(string message)
{
    public string Message { get; private set; } = message;
}