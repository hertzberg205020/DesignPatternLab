using YoutubeSubscribe.Models;

namespace YoutubeSubscribe.Strategies;

public interface IReceivedNotificationStrategy
{
    void ReceivedNotification(Video video);

    public IChannelObserver ChannelObserver { get; set; }
}