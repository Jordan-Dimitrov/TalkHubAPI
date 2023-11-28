namespace TalkHubAPI.Models.ConfigurationModels
{
    public class FFMpegConfig
    {
        public string FFMpegBinaryDirectory { get; set; } = null!;
        public string TemporaryFilesDirectory { get; set; } = null!;
        public int VideoConversionThreads { get; set; }
        public int PhotoConversionThreads { get; set; }
    }
}
