﻿using System.ComponentModel.DataAnnotations;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Models.MessengerModels;
using TalkHubAPI.Models.PhotosManagerModels;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Models;

public enum UserRole
{
    Visitor,
    User,
    Admin
}

public partial class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters")]
    public string Username { get; set; } = null!;
    [Required]
    public byte[] PasswordHash { get; set; } = null!;
    [Required]
    public byte[] PasswordSalt { get; set; } = null!;

    public int? RefreshTokenId { get; set; }
    [Required]
    [Range(0, 1)]
    public UserRole PermissionType { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    public string? VerificationToken { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public string? PasswordResetToken { get; set; }

    public DateTime? ResetTokenExpires { get; set; }
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
    public int SubscriberCount { get; set; }

    public virtual ICollection<ForumMessage> ForumMessages { get; set; } = new List<ForumMessage>();

    public virtual ICollection<MessengerMessage> MessengerMessages { get; set; } = new List<MessengerMessage>();

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual RefreshToken? RefreshToken { get; set; }

    public virtual ICollection<UserMessageRoom> UserMessageRooms { get; set; } = new List<UserMessageRoom>();

    public virtual ICollection<UserUpvote> UserUpvotes { get; set; } = new List<UserUpvote>();
    public virtual ICollection<UserSubscribtion> UserSubscribtionUserChannels { get; set; } = new List<UserSubscribtion>();

    public virtual ICollection<UserSubscribtion> UserSubscribtionUserSubscribers { get; set; } = new List<UserSubscribtion>();

    public virtual ICollection<VideoComment> VideoComments { get; set; } = new List<VideoComment>();

    public virtual ICollection<VideoCommentsLike> VideoCommentsLikes { get; set; } = new List<VideoCommentsLike>();

    public virtual ICollection<VideoUserLike> VideoUserLikes { get; set; } = new List<VideoUserLike>();

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
