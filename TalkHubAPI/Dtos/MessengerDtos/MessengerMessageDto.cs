﻿using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Dtos.MessengerDtos
{
    public class MessengerMessageDto
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [StringLength(255, MinimumLength = 3, ErrorMessage = "Message must be between 3 and 255 characters")]
        public string? MessageContent { get; set; }

        public string? FileName { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
    }
}
