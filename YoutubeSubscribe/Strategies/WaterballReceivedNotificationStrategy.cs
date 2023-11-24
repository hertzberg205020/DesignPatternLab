using YoutubeSubscribe.Models;

namespace YoutubeSubscribe.Strategies;

public class WaterballReceivedNotificationStrategy: IReceivedNotificationStrategy
{
    public IChannelObserver? ChannelObserver { get; set; }
    
    public void ReceivedNotification(Video video)
    {
        // 如果影片的長度時間 ≥ 三分鐘，
        // 那麼水球就會對其影片按讚 (Like)，
        // 否則置之不理。
        if (video.Length >= 180)
        {
            Console.WriteLine($"水球 對影片 {video.Title} 按讚");
            video.NumOfLikes++;
        }
    }
}