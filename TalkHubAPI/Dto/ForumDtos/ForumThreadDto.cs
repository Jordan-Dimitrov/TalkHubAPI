﻿namespace TalkHubAPI.Dto.ForumDtos
{
    public class ForumThreadDto
    {
        public int Id { get; set; }

        public string ThreadName { get; set; } = null!;

        public string ThreadDescription { get; set; } = null!;
    }
}