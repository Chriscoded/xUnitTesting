using System.Threading.Tasks;
using xUnitTesting.Controllers;
using xUnitTesting.Services;
using TestProject1.MockData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TestProject1.System.Controllers;

public class TestTodoController
{
    [Fact]
    public async Task GetAllAsync_ShouldReturn200Status()
    {
        /// Arrange
        /// we create a mock instance of 'ITodoService
        var todoService = new Mock<ITodoService>();

        //then assign mock the result to 'ITodoService.GetAllAsync() from TodoMockData.GetTodos()
        todoService.Setup(_ => _.GetAllAsync()).ReturnsAsync(TodoMockData.GetTodos());

        //Finally creates the instance of 'TodoController
        //Here variable 'sut' means 'System Under Test' just a recommended naming convention.
        var sut = new TodoController(todoService.Object);

        //Here invoking our  controllers action method 'GetAllAsync()'. Since our action method returns 'OkObjectResult'
        //for 200 status so here we explicitly typecasting the result.
        /// Act
        var result = (OkObjectResult)await sut.GetAllAsync();


        // /// Assert
        result.StatusCode.Should().Be(200);
    }

    // existing cod hidden for display purpose
    [Fact]
    public async Task GetAllAsync_ShouldReturn204NoContentStatus()
    {
        /// Arrange
        var todoService = new Mock<ITodoService>();
        todoService.Setup(_ => _.GetAllAsync()).ReturnsAsync(TodoMockData.GetEmptyTodos());
        var sut = new TodoController(todoService.Object);

        /// Act
        var result = (NoContentResult)await sut.GetAllAsync();


        /// Assert
        result.StatusCode.Should().Be(204);
        todoService.Verify(_ => _.GetAllAsync(), Times.Exactly(1));
    }

    [Fact]
    public async Task SaveAsync_ShouldCall_ITodoService_SaveAsync_AtleastOnce()
    {
        /// Arrange
        var todoService = new Mock<ITodoService>();
        var newTodo = TodoMockData.NewTodo();
        var sut = new TodoController(todoService.Object);

        /// Act
        var result = await sut.SaveAsync(newTodo);

        /// Assert
        todoService.Verify(_ => _.SaveAsync(newTodo), Times.Exactly(1));
    }
}
