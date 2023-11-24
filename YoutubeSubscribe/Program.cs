// See https://aka.ms/new-console-template for more information

using YoutubeSubscribe.Models;
using YoutubeSubscribe.Strategies;

var pewDiePie = new Channel("PewDiePie");

var waterballCollege = new Channel("Waterball College");

var waterballStrategy = new WaterballReceivedNotificationStrategy();
var waterballSubscriber = new ChannelSubscriber("Waterball Subscriber", waterballStrategy);

var fireballStrategy = new FireballReceivedNotificationStrategy();
var fireballSubscriber = new ChannelSubscriber("Fireball Subscriber", fireballStrategy);

waterballSubscriber.Subscribe(pewDiePie);
waterballSubscriber.Subscribe(waterballCollege);

fireballSubscriber.Subscribe(pewDiePie);
fireballSubscriber.Subscribe(waterballCollege);

var video1 = new Video()
{
    Title = "C1M1S2",
    Description = "這個世界正是物件導向的呢！",
    Length = 240
};

var video2 = new Video()
{
    Title = "Hello guys",
    Description = "Clickbait",
    Length = 30
};

var video3 = new Video()
{
    Title = "C1M1S3",
    Description = "物件 vs. 類別",
    Length = 60
};

var video4 = new Video()
{
    Title = "Minecraft",
    Description = "Let’s play Minecraft",
    Length = 1800
};

waterballCollege.UploadVideo(video1);
pewDiePie.UploadVideo(video2);
waterballCollege.UploadVideo(video3);
pewDiePie.UploadVideo(video4);

