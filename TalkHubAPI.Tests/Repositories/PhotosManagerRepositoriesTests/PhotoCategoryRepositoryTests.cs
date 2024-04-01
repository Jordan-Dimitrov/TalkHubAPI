using FluentAssertions;
using TalkHubAPI.Data;
using TalkHubAPI.Models.PhotosManagerModels;
using TalkHubAPI.Repositories.PhotosManagerRepositories;

namespace TalkHubAPI.Tests.Repositories.PhotosManagerRepositoriesTests
{
    public class PhotoCategoryRepositoryTests
    {
        private readonly Seeder _Seeder;
        public PhotoCategoryRepositoryTests()
        {
            _Seeder = new Seeder();
        }

        [Fact]
        public async Task PhotoCategoryRepository_GetCategoryAsync_ReturnsPhotoCategory()
        {
            int photoCategoryId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            PhotoCategoryRepository photoCategoryRepository = new PhotoCategoryRepository(context);

            PhotoCategory result = await photoCategoryRepository.GetCategoryAsync(photoCategoryId);

            result.Should().NotBeNull();
            result.Should().BeOfType<PhotoCategory>();
        }
    }
}
