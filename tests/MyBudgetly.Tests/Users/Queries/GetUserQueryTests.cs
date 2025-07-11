using FluentAssertions;
using Moq;
using MyBudgetly.Application.Users.Dto.Mappers;
using MyBudgetly.Application.Users.Queries;
using MyBudgetly.Domain.Common.Exceptions;
using MyBudgetly.Domain.Users;
using MyBudgetly.Domain.Users.Exceptions;

namespace MyBudgetly.Tests.Users.Queries;

public class GetUserQueryTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly UserDtoMapper _mapper = new(); // без залежностей

    [Fact]
    public async Task Handle_Should_Return_UserDto_When_User_Exists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User("thor@asgard.com")
        {
            FirstName = "Thor",
            LastName = "Odinson"
        };

        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new GetUserQuery.Handler(_userRepositoryMock.Object, _mapper);
        var query = new GetUserQuery.Message { UserId = userId };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be("Thor");
        result.LastName.Should().Be("Odinson");
    }

    [Fact]
    public async Task Handle_Should_Throw_When_User_Not_Found()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new UserNotFoundException(userId));

        var handler = new GetUserQuery.Handler(_userRepositoryMock.Object, _mapper);
        var query = new GetUserQuery.Message { UserId = userId };

        // Act & Assert
        await FluentActions.Invoking(() => handler.Handle(query, CancellationToken.None))
            .Should()
            .ThrowAsync<ExceptionWithStatusCode>()
            .WithMessage("*not found*");
    }
}