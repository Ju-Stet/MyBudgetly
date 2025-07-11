using FluentAssertions;
using Moq;
using MyBudgetly.Application.Users.Commands;
using MyBudgetly.Domain.Common.Exceptions;
using MyBudgetly.Domain.Users;

namespace MyBudgetly.Tests.Users.Commands;

public class DeleteUserCommandTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly DeleteUserCommand.Handler _handler;

    public DeleteUserCommandTests()
    {
        _handler = new DeleteUserCommand.Handler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Delete_User_When_User_Exists()
    {
        // Arrange
        var existingUser = new User("natasha@shield.gov");

        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(existingUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        var command = new DeleteUserCommand.Message { UserId = existingUser.Id };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _userRepositoryMock.Verify(r => r.RemoveAsync(existingUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_User_Not_Found()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null!);

        var command = new DeleteUserCommand.Message { UserId = userId };

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<ExceptionWithStatusCode>()
            .WithMessage($"*{userId}*not found*");

        _userRepositoryMock.Verify(r => r.RemoveAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _userRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}