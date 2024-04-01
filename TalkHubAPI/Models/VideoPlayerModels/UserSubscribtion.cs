namespace TalkHubAPI.Models.VideoPlayerModels;

public partial class UserSubscribtion
{
    public int Id { get; set; }
    public int? UserSubscriberId { get; set; }
    public int? UserChannelId { get; set; }
    public virtual User? UserChannel { get; set; }
    public virtual User? UserSubscriber { get; set; }
}
