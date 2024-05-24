using System.Linq;
using Microsoft.AspNetCore.Identity;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoItems;
using Todo.Tests.MockDataBuilder;
using Xunit;

namespace Todo.Tests.ConverterTest
{
    public class WhenTodoItemIsConvertedToEditFields
    {
        private readonly TodoItem srcTodoItem;
        private readonly TodoItemEditFields resultFields;

        public WhenTodoItemIsConvertedToEditFields()
        {
            var todoList = new TestTodoListBuilder(new IdentityUser("alice@example.com"), "shopping")
                    .WithItem("bread", Importance.High)
                    .Build();

            srcTodoItem = todoList.Items.First();

            resultFields = TodoItemEditFieldsFactory.Create(srcTodoItem);
        }

        [Fact]
        public void EqualTodoListId()
        {
            Assert.Equal(srcTodoItem.TodoListId, resultFields.TodoListId);
        }

        [Fact]
        public void EqualTitle()
        {
            Assert.Equal(srcTodoItem.Title, resultFields.Title);
        }

        [Fact]
        public void EqualImportance()
        {
            Assert.Equal(srcTodoItem.Importance, resultFields.Importance);
        }
    }
}