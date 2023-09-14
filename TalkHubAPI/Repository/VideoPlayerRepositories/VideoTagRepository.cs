using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repository.VideoPlayerRepositories
{
    public class VideoTagRepository : IVideoTagRepository
    {
        private readonly TalkHubContext _Context;
        public VideoTagRepository(TalkHubContext context)
        {
            _Context = context;
        }
        public bool AddVideoTag(VideoTag tag)
        {
            _Context.Add(tag);
            return Save();
        }

        public VideoTag GetVideoTag(int id)
        {
            return _Context.VideoTags.Find(id);
        }

        public VideoTag GetVideoTagByName(string name)
        {
            return _Context.VideoTags.FirstOrDefault(x => x.TagName == name);
        }

        public ICollection<VideoTag> GetVideoTags()
        {
            return _Context.VideoTags.ToList();
        }

        public bool RemoveVideoTag(VideoTag tag)
        {
            _Context.Remove(tag);
            return Save();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateVideoTag(VideoTag tag)
        {
            _Context.Update(tag);
            return Save();
        }

        public bool VideoTagExists(int id)
        {
            return _Context.VideoTags.Any(x => x.Id == id);
        }

        public bool VideoTagExists(string name)
        {
            return _Context.VideoTags.Any(x => x.TagName == name);
        }
    }
}
