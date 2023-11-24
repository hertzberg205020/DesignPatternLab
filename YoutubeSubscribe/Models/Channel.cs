namespace YoutubeSubscribe.Models;

public class Channel
{
    public string? Name { get; set; }
    
    private readonly List<ChannelSubscriber> _subscribers = new();
    
    private readonly List<Video> _videos = new();

    public Channel(string name)
    {
        Name = name;
    }
    
    public void Subscribe(ChannelSubscriber channelSubscriber)
    {
        _subscribers.Add(channelSubscriber);
    }

    public void UnSubscribe(ChannelSubscriber channelSubscriber)
    {
        var target = _subscribers.Find(c => c.Name == channelSubscriber.Name);

        if (target != null)
        {
            _subscribers.Remove(target);
        }
    }
    
    /// <summary>
    /// upload video to channel
    /// </summary>
    /// <param name="video"></param>
    public void UploadVideo(Video video)
    {
        Console.WriteLine($"頻道 {Name} 上傳影片 {video.Title}");
        _videos.Add(video);
        video.Channel = this;  // the Association relationship between channel and video is required
        NotifyObservers(video);
    }

    /// <summary>
    /// notify all subscribers
    /// </summary>
    /// <param name="video"></param>
    public void NotifyObservers(Video video)
    {
        var subscribersSnapshot = new List<ChannelSubscriber>(_subscribers);
        
        foreach (var subscriber in subscribersSnapshot)
        {
            subscriber.ReceivedNotification(video);
        }
    }
}