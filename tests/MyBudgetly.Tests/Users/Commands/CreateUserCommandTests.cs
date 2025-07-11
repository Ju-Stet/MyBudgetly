using FluentAssertions;
using Moq;
using MyBudgetly.Application.Users;
using MyBudgetly.Application.Users.Commands;
using MyBudgetly.Application.Users.Dto.Models;
using MyBudgetly.Domain.Common.Exceptions;
using MyBudgetly.Domain.Users;

namespace MyBudgetly.Tests.Users.Commands;

public class CreateUserCommandTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IUserUniquenessChecker> _uniquenessCheckerMock = new();
    private readonly UserApplicationService _userApplicationService;
    private readonly UserDomainService _userDomainService;

    public CreateUserCommandTests()
    {
        _userApplicationService = new UserApplicationService();
        _userDomainService = new UserDomainService(_uniquenessCheckerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_User_When_Email_Is_Unique()
    {
        // Arrange
        var handler = new CreateUserCommand.Handler(
            _userRepositoryMock.Object,
            _userApplicationService,
            _userDomainService
        );

        var userDto = new CreateUserDto
        {
            FirstName = "Tony",
            LastName = "Stark",
            Email = "tony@starkindustries.com",
            BackupEmail = "ironman@avengers.com"
        };

        _uniquenessCheckerMock
            .Setup(x => x.IsEmailUsedByAnotherUserAsync(It.IsAny<Guid?>(), userDto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new CreateUserCommand.Message
        {
            UserDto = userDto
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty(); // Guid not empty
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Email_Is_Not_Unique()
    {
        // Arrange
        var handler = new CreateUserCommand.Handler(
            _userRepositoryMock.Object,
            _userApplicationService,
            _userDomainService
        );

        var userDto = new CreateUserDto
        {
            FirstName = "Bruce",
            LastName = "Banner",
            Email = "hulk@avengers.com",
            BackupEmail = "green@rage.com"
        };

        _uniquenessCheckerMock
            .Setup(x => x.IsEmailUsedByAnotherUserAsync(It.IsAny<Guid?>(), userDto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true); // Email already in use

        var command = new CreateUserCommand.Message
        {
            UserDto = userDto
        };

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<ExceptionWithStatusCode>()
            .WithMessage($"Email '{userDto.Email}' is already in use.");

        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Throw_DomainException_When_BackupEmail_Equals_Email()
    {
        // Arrange
        var handler = new CreateUserCommand.Handler(
            _userRepositoryMock.Object,
            _userApplicationService,
            _userDomainService
        );

        var userDto = new CreateUserDto
        {
            FirstName = "Steve",
            LastName = "Rogers",
            Email = "cap@avengers.com",
            BackupEmail = "cap@avengers.com" // Same as primary
        };

        _uniquenessCheckerMock
            .Setup(x => x.IsEmailUsedByAnotherUserAsync(null, userDto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new CreateUserCommand.Message { UserDto = userDto };

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("Backup email must differ from the primary email.");

        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}