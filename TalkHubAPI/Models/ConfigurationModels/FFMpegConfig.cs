namespace TalkHubAPI.Models.ConfigurationModels
{
    public class FFMpegConfig
    {
        public string FFMpegBinaryDirectory { get; set; }
        public string TemporaryFilesDirectory { get; set; }
        public int VideoConversionThreads { get; set; }
        public int PhotoConversionThreads { get; set; }
    }
}
