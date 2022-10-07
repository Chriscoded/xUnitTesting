using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xUnitTesting.Data;
using xUnitTesting.Services;
using TestProject1.MockData;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TestProject1.System.Controllers
{
    public class TestTodoService : IDisposable
    {
        protected readonly MyWorldDbContext _context;
        public TestTodoService()
        {
            //GUID stands for Global Unique Identifier
            //Here constructor gets invoked for every execution of the test method.
            //So we want to create a separate in-memory database for each uni test method, so we have
            //to define the unique database name which we are going to achieve by using the 'Guid'.
            var options = new DbContextOptionsBuilder<MyWorldDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            _context = new MyWorldDbContext(options);

            //The 'EnsureCreated()' method to determine database for the context is exist.
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllAsync_ReturnTodoCollection()
        {
            //Here seeding some test data into the 'Todo' table of our in-memory database
            /// Arrange
            _context.Todo.AddRange(MockData.TodoMockData.GetTodos());
            _context.SaveChanges();

            //Creating the object for our 'TodoService'.
            var sut = new TodoService(_context);

            //Invoking our test method that is 'TodoService.GetAllAsync()'.
            /// Act
            var result = await sut.GetAllAsync();

            //Verifying the output as expected or not.
            /// Assert
            result.Should().HaveCount(TodoMockData.GetTodos().Count);
        }

        [Fact]
        public async Task SaveAsync_AddNewTodo()
        {
            /// Arrange
            var newTodo = TodoMockData.NewTodo();
            _context.Todo.AddRange(MockData.TodoMockData.GetTodos());
            _context.SaveChanges();

            var sut = new TodoService(_context);

            /// Act
            await sut.SaveAsync(newTodo);

            ///Assert
            int expectedRecordCount = (TodoMockData.GetTodos().Count() + 1);
            _context.Todo.Count().Should().Be(expectedRecordCount);
        }

        //The 'Dispose' method gets executed on completion of test case method execution. So here we want
        //to destroy the in-memory database so that every test case will have its own in-memory database
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }

}

