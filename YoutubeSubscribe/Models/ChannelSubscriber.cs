
using YoutubeSubscribe.Strategies;

namespace YoutubeSubscribe.Models;

public class ChannelSubscriber: IChannelObserver
{
    private readonly IReceivedNotificationStrategy _receivedNotificationStrategy;
    
    private readonly List<Channel> _subscribedChannels = new();

    public string? Name { get; set; }

    public ChannelSubscriber(string name, IReceivedNotificationStrategy receivedNotificationStrategy)
    {
        Name = name;
        _receivedNotificationStrategy = receivedNotificationStrategy;
        _receivedNotificationStrategy.ChannelObserver = this;
    }
    
    public void ReceivedNotification(Video video)
    {
        _receivedNotificationStrategy.ReceivedNotification(video);
    }

    public void Subscribe(Channel channel)
    {
        Console.WriteLine($"{this.Name} 訂閱頻道 {channel.Name}");
        _subscribedChannels.Add(channel);
        channel.Subscribe(this);
    }

    public void UnSubscribe(Channel channel)
    {
        var target = _subscribedChannels.Find(c => c.Name == channel.Name);

        if (target != null)
        {
            Console.WriteLine($"{Name} 解除訂閱了 {channel.Name}");
            _subscribedChannels.Remove(target);
            channel.UnSubscribe(this);
        }
    }
}