namespace FlightManagement.Notifications.Abstractions;

/// <summary>
/// Base interface for all notification handlers
/// </summary>
public interface INotificationHandler<in TNotification> where TNotification : INotification
{
    Task SendAsync(TNotification notification, CancellationToken cancellationToken = default);
}

/// <summary>
/// Marker interface for notifications
/// </summary>
public interface INotification
{
    string Recipient { get; }
}

