using FluentAssertions;
using Moq;
using MyBudgetly.Application.Users.Commands;
using MyBudgetly.Application.Users.Dto.Models;
using MyBudgetly.Domain.Common.Exceptions;
using MyBudgetly.Domain.Users;

namespace MyBudgetly.Tests.Users.Commands;

public class UpdateUserCommandTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly UserDomainService _userDomainService;

    public UpdateUserCommandTests()
    {
        _userDomainService = new UserDomainService(new Mock<IUserUniquenessChecker>().Object);
    }

    [Fact]
    public async Task Handle_Should_Update_User_When_User_Exists()
    {
        // Arrange
        var existingUser = new User("bruce.banner@avengers.com");
        existingUser.UpdateProfile("Bruce", "Banner", null);

        var handler = new UpdateUserCommand.Handler(
            _userRepositoryMock.Object,
            _userDomainService
        );

        _userRepositoryMock.Setup(r => r.GetByIdAsync(existingUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        var command = new UpdateUserCommand.Message
        {
            UserId = existingUser.Id,
            UserDto = new UpdateUserDto
            {
                FirstName = "Hulk",
                LastName = "Smash",
                BackupEmail = "hulk@green.com"
            }
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        existingUser.FirstName.Should().Be("Hulk");
        existingUser.LastName.Should().Be("Smash");
        existingUser.BackupEmail.Should().Be("hulk@green.com");

        _userRepositoryMock.Verify(r => r.UpdateAsync(existingUser, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnFalse_When_User_Not_Found()
    {
        // Arrange
        var handler = new UpdateUserCommand.Handler(
            _userRepositoryMock.Object,
            _userDomainService
        );

        _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null!);

        var command = new UpdateUserCommand.Message
        {
            UserId = Guid.NewGuid(),
            UserDto = new UpdateUserDto
            {
                FirstName = "Dummy",
                LastName = "User",
                BackupEmail = null
            }
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        _userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _userRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_BackupEmail_Equals_Email()
    {
        // Arrange
        var existingUser = new User("clint@avengers.com");
        existingUser.UpdateProfile("Clint", "Barton", null);

        var handler = new UpdateUserCommand.Handler(
            _userRepositoryMock.Object,
            _userDomainService
        );

        _userRepositoryMock.Setup(r => r.GetByIdAsync(existingUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        var command = new UpdateUserCommand.Message
        {
            UserId = existingUser.Id,
            UserDto = new UpdateUserDto
            {
                FirstName = "Hawkeye",
                LastName = "Agent",
                BackupEmail = "clint@avengers.com" // == primary email
            }
        };

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*must differ*");

        _userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.
            Never);
        _userRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}