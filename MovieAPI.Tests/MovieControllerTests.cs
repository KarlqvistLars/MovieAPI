using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieAPI.DTOs;
using MovieAPI.Interfaces;

namespace MovieAPI.Tests
{
    public class MovieControllerTests
    {
        [Fact]
        public async Task GetMovies_ReturnsOkWithMovies()
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

        [Fact]
        public async Task GetMovieById_ReturnsOkWithMovie()
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
        public async Task GetMovieDetails_ReturnsOK()
        {
            // Arrange
            var movieDto = new MovieDto {
                Id = 1,
                Title = "Test Movie",
                Year = null,
                Duration = null,
                Details = new MovieDetailsDto {
                    Synopsis = "SynopsisTest.",
                    Language = "Engelska",
                    Budget = "$Test"
                },
                Actors = new List<ActorDto>(),
                Genres = new List<GenreDto>(),
                Reviews = new List<ReviewDto>()
            };

            var mockService = new Mock<IMovieService>();

            mockService
                .Setup(s => s.GetMovieDetails(1))
                .ReturnsAsync(movieDto);

            var controller = new MoviesController(mockService.Object);

            // Act
            var result = await controller.GetMovieDetails(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedMovie = Assert.IsType<MovieDetailsDto>(okResult.Value);
            Assert.Equal("Test Movie", returnedMovie.Title);

            var returnedDetails = Assert.IsType<MovieDetailsDto>(okResult.Value);
            Assert.Equal("SynopsisTest.", returnedDetails.Synopsis);
            Assert.Equal("Engelska", returnedDetails.Language);
            Assert.Equal("$Test", returnedDetails.Budget);
        }

        [Fact]
        public async Task GetMovieReviewByMovieId_ReturnsOKWithReviews()
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
                Reviews = new List<ReviewDto> {
                    new ReviewDto {
                        ReviewId = 1,
                        ReviewerName = "Test Reviewer",
                        Rating = 5,
                        Comment = "Great movie!"
                    }
                }
            };

            var mockService = new Mock<IMovieService>();

            mockService
                .Setup(s => s.GetMovieReviews(1))
                .ReturnsAsync(movieDto);

            var controller = new MoviesController(mockService.Object);

            // Act
            var result = await controller.GetMovieReviews(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var reviews = Assert.IsType<List<ReviewDto>>(okResult.Value);

            var returnedReviews = Assert.IsType<List<ReviewDto>>(reviews);
            Assert.Single(returnedReviews);
            //Assert.Equal(1, returnedReviews[0].Id);
            Assert.Equal("Test Reviewer", returnedReviews[0].ReviewerName);
            Assert.Equal(5, returnedReviews[0].Rating);
            Assert.Equal("Great movie!", returnedReviews[0].Comment);
        }
    }
}
