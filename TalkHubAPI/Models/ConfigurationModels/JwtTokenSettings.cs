﻿namespace TalkHubAPI.Models.ConfigurationModels
{
    public class JwtTokenSettings
    {
        public string Token { get; set; } = null!;
        public int MinutesExpiry { get; set; }
    }
}
