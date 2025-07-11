using FluentAssertions;
using Moq;
using MyBudgetly.Application.Users.Dto.Mappers;
using MyBudgetly.Application.Users.Queries;
using MyBudgetly.Domain.Users;

namespace MyBudgetly.Tests.Users.Queries;

public class GetAllUsersQueryTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly UserDtoMapper _mapper = new(); // якщо не має залежностей

    [Fact]
    public async Task Handle_Should_Return_All_Users_As_Dto()
    {
        // Arrange
        var domainUsers = new List<User>
        {
            new("steve@avengers.com") { FirstName = "Steve", LastName = "Rogers" },
            new("natasha@avengers.com") { FirstName = "Natasha", LastName = "Romanoff" }
        };

        _userRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(domainUsers);

        var handler = new GetAllUsersQuery.Handler(_userRepositoryMock.Object, _mapper);
        var query = new GetAllUsersQuery.Message();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result[0].FirstName.Should().Be("Steve");
        result[1].LastName.Should().Be("Romanoff");

        _userRepositoryMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}