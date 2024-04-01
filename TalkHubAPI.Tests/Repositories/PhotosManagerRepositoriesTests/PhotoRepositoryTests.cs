using FluentAssertions;
using TalkHubAPI.Data;
using TalkHubAPI.Models.PhotosManagerModels;
using TalkHubAPI.Repositories.PhotosManagerRepositories;

namespace TalkHubAPI.Tests.Repositories.PhotosManagerRepositoriesTests
{
    public class PhotoRepositoryTests
    {
        private readonly Seeder _Seeder;
        public PhotoRepositoryTests()
        {
            _Seeder = new Seeder();
        }

        [Fact]
        public async Task PhotoRepository_GetPhotoAsync_ReturnsPhoto()
        {
            int photoId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            PhotoRepository photoRepository = new PhotoRepository(context);

            Photo result = await photoRepository.GetPhotoAsync(photoId);

            result.Should().NotBeNull();
            result.Should().BeOfType<Photo>();
        }
    }
}
