using Moq;
using Xunit;
using CharacterApp.Data;
using CharacterApp.Models;
using CharacterApp.Services;

using Microsoft.Extensions.Logging;

namespace CharacterApp.Tests
{
    public class SpeciesServiceTests
    {
        [Fact]
        public async Task CreateSpeciesAsync_ThrowsFormatException_WhenSpeciesObjectHasNonNullOrEmptyId()
        {
            // Arrange
            var repo = new Mock<ISpeciesRepository>();
            var logger = new Mock<ILogger<SpeciesService>>();
            var service = new SpeciesService(repo.Object, logger.Object);

            var species = new Species { Id = 1 };

            // Act
            var ex = await Assert.ThrowsAsync<FormatException>(
                () => service.CreateSpeciesAsync(species));

            // Assert
            Assert.Equal("New species object cannot contain hardcoded id", ex.Message);
        }

        [Fact]
        public async Task CreateSpeciesAsync_ReturnsCreatedSpeciesObject_WhenSpeciesObjectIsValid()
        {
            // Arrange
            var repo = new Mock<ISpeciesRepository>();
            var logger = new Mock<ILogger<SpeciesService>>();
            var service = new SpeciesService(repo.Object, logger.Object);

            var species = new Species { Name = "Test" };

            repo.Setup(r => r.CreateSpeciesAsync(It.IsAny<Species>()))
                .ReturnsAsync(species);

            // Act
            var result = await service.CreateSpeciesAsync(species);

            // Assert
            Assert.Equal(species, result);
        }

        [Fact]
        public async Task DeleteSpeciesAsync_ReturnsDeletedSpeciesObject_WhenSpeciesObjectExists()
        {
            // Arrange
            var repo = new Mock<ISpeciesRepository>();
            var logger = new Mock<ILogger<SpeciesService>>();
            var service = new SpeciesService(repo.Object, logger.Object);

            var species = new Species { Id = 1 };

            repo.Setup(r => r.GetSpeciesByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    if (id == species.Id)
                    {
                        return species;
                    }
                    else
                    {
                        throw new KeyNotFoundException("Species with the id " + id + " was not found");
                    }
                });

            repo.Setup(r => r.DeleteSpeciesAsync(It.IsAny<Species>())).ReturnsAsync(species);
            // Act
            var result = await service.DeleteSpeciesAsync(1);

            // Assert
            Assert.Equal(species, result);
        }

        [Fact]
        public async Task DeleteSpeciesAsync_ThrowsKeyNotFoundException_WhenSpeciesObjectDoesNotExist()
        {
            // Arrange
            var repo = new Mock<ISpeciesRepository>();
            var logger = new Mock<ILogger<SpeciesService>>();
            var service = new SpeciesService(repo.Object, logger.Object);

            // Act
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => service.DeleteSpeciesAsync(1));

            // Assert
            Assert.Equal("Speice with the id 1 was not found", ex.Message);
        }
    }
}
