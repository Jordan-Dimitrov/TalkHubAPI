namespace TalkHubAPI.Models
{
    public class VideoUploadResponse
    {
        public Guid? TaskId { get; set; }
        public string? WebmFileName { get; set; }
        public string? Error { get; set; }
    }
}
