using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieAPI.DTOs;
using MovieAPI.Interfaces;

namespace MovieAPI.Tests
{
    public class MovieAPIControllerTests
    {

        [Fact]
        public async Task Get_ById_ReturnsOkWithMovie()
        {

            // Arrange
            var movieDto = new MovieDto {
                Id = 1,
                Title = "Test Movie",
                Year = null,
                Duration = null,
                Details = new MovieDetailsDto(),
                Actors = new List<ActorDto>(),
                Genres = new List<GenreDto>(),
                Reviews = new List<ReviewDto>()
            };

            var mockService = new Mock<IMovieService>();

            mockService
                .Setup(s => s.GetMovieById(1))
                .ReturnsAsync(movieDto);

            var controller = new MoviesController(mockService.Object);

            // Act
            var result = await controller.GetMovieById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var returnedMovie = Assert.IsType<MovieDto>(okResult.Value);

            Assert.Equal(1, returnedMovie.Id);
            Assert.Equal("Test Movie", returnedMovie.Title);
        }

        [Fact]
        public async Task Get_ReturnsOkWithMovies()
        {

            // Arrange
            ICollection<MovieDto> movies = new List<MovieDto>()
            {
                new MovieDto {
                Id = 1,
                Title = "Test Movie",
                Year = null,
                Duration = null,
                Details = new MovieDetailsDto(),
                Actors = new List<ActorDto>(),
                Genres = new List<GenreDto>(),
                Reviews = new List<ReviewDto>() },
                new MovieDto {
                Id = 2,
                Title = "Test Movie 2",
                Year = null,
                Duration = null,
                Details = new MovieDetailsDto(),
                Actors = new List<ActorDto>(),
                Genres = new List<GenreDto>(),
                Reviews = new List<ReviewDto>() }
            };

            var mockService = new Mock<IMovieService>();

            mockService
                .Setup(s => s.GetMovies())
                .ReturnsAsync(movies);

            var controller = new MoviesController(mockService.Object);

            // Act
            var result = await controller.GetMovies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var returnedMovies = Assert.IsType<List<MovieDto>>(okResult.Value);

            Assert.Equal(2, returnedMovies.Count);
            Assert.Equal(1, returnedMovies[0].Id);
            Assert.Equal("Test Movie", returnedMovies[0].Title);
            Assert.Equal(2, returnedMovies[1].Id);
            Assert.Equal("Test Movie 2", returnedMovies[1].Title);
        }
    }
}
