using YoutubeSubscribe.Models;

namespace YoutubeSubscribe.Strategies;

public class FireballReceivedNotificationStrategy: IReceivedNotificationStrategy
{
    public IChannelObserver? ChannelObserver { get; set; }
    
    public void ReceivedNotification(Video video)
    {
        // 如果影片的長度時間 ≤ 一分鐘，
        // 那麼火球就會立刻對該頻道解除訂閱 (Unsubscribe)
        if (video.Length <= 60)
        {
            ChannelObserver?.UnSubscribe(video.Channel);
        }
    }
    
}