using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieAPI.DTOs;
using MovieAPI.Interfaces;

namespace MovieAPI.Tests
{
    public class ActorControllerTests
    {
        [Fact]
        public async Task GetActors_ReturnsOkWithActors()
        {

            // Arrange
            ICollection<ActorDto> actors = new List<ActorDto>()
            {
                new ActorDto {
                ActorId = 1,
                Name = "Test Actor",
                BirthYear = "1980",
                },
                new ActorDto {
                ActorId = 2,
                Name = "Test Actor 2",
                BirthYear = "1990",
                }
            };

            var mockService = new Mock<IActorService>();

            mockService
                .Setup(s => s.GetActors())
                .ReturnsAsync(actors);

            var controller = new ActorsController(mockService.Object);

            // Act
            var result = await controller.GetActors();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var returnedActors = Assert.IsType<List<ActorDto>>(okResult.Value);

            Assert.Equal(2, returnedActors.Count);
            Assert.Equal(1, returnedActors[0].ActorId);
            Assert.Equal("Test Actor", returnedActors[0].Name);
            Assert.Equal(2, returnedActors[1].ActorId);
            Assert.Equal("Test Actor 2", returnedActors[1].Name);
        }

        [Fact]
        public async Task GetActorById_ReturnsOkWithActor()
        {
            // Arrange
            var actorDto = new ActorDto {
                ActorId = 1,
                Name = "Test Actor",
                BirthYear = "1980"
            };

            var mockService = new Mock<IActorService>();

            mockService
                .Setup(s => s.GetActor(1))
                .ReturnsAsync(actorDto);

            var controller = new ActorsController(mockService.Object);

            // Act
            var result = await controller.GetActor(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var returnedActor = Assert.IsType<ActorDto>(okResult.Value);

            Assert.Equal(1, returnedActor.ActorId);
            Assert.Equal("Test Actor", returnedActor.Name);
        }
    }
}
