﻿using TalkHubAPI.Models.VideoPlayerModels;
namespace TalkHubAPI.Interfaces.VideoPlayerInterfaces
{
    public interface IVideoRepository
    {
        bool AddVideo(Video video);
        bool RemoveVideo(Video video);
        bool UpdateVideo(Video video);
        bool VideoExists(int id);
        bool Save();
        Video GetVideo(int id);
        ICollection<Video> GetVideos();
        bool VideoExists(string name);
        Video GetVideoByName(string name);
        ICollection<Video> GetVideosByTagId(int tagId);
        ICollection<Video> GetVideosByUserId(int userId);
        ICollection<Video> GetVideosByPlaylistId(int playlistId);
    }
}
