namespace YoutubeSubscribe.Models;

public interface IChannelObserver
{
    public String? Name { get; set; }
    
    void ReceivedNotification(Video video);
    
    void Subscribe(Channel channel);
    
    void UnSubscribe(Channel channel);
}