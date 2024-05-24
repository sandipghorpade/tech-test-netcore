using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using Todo.Controllers;
using Todo.Data;
using Todo.Models.TodoItems;
using Xunit;

namespace Todo.Tests.ControllerTest
{
    public class ToDoItemControllerTest : AutoDataAttribute
    {
        private readonly IFixture _fixture;
        private readonly Mock<ApplicationDbContext> _mockDbContext;
        private readonly TodoItemController _todoItemController;
        public ToDoItemControllerTest()
        {
            _fixture = new Fixture();
            _mockDbContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _todoItemController = new TodoItemController(_mockDbContext.Object); 
        }

        [Fact]
        public async Task TodoItemCreateTest()
        {
            TodoItemCreateFields todoItemCreateFields = _fixture.Create<TodoItemCreateFields>();
            IActionResult result = await _todoItemController.Create(todoItemCreateFields);
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);
            redirectResult.ActionName.Should().Be("Detail");
        }

    }
}
