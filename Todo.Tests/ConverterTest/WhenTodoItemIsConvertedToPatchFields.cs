using Microsoft.AspNetCore.Identity;
using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoItems;
using Todo.Tests.MockDataBuilder;
using Xunit;

namespace Todo.Tests.ConverterTest
{
    public class WhenTodoItemIsConvertedToPatchFields
    {
        private readonly TodoItem _srcTodoItem;
        private readonly TodoItemPatchFields _resultFields;

        public WhenTodoItemIsConvertedToPatchFields()
        {
            var todoList = new TestToDoListFixtureBuilder("TestPatchFieldConverter")
                    .WithItem()
                    .Build();

            _srcTodoItem = todoList.Items.First();

            _resultFields = TodoItemPatchFieldsFactory.Create(_srcTodoItem);
        }


        [Fact]
        public void EqualTodoListId()
        {
            Assert.Equal(_srcTodoItem.TodoListId, _resultFields.TodoListId);
        }

        [Fact]
        public void EqualTitle()
        {
            Assert.Equal(_srcTodoItem.Title, _resultFields.Title);
        }

        [Fact]
        public void EqualImportance()
        {
            Assert.Equal(_srcTodoItem.Importance, _resultFields.Importance);
        }

        [Fact]
        public void EqualRank()
        {
            Assert.Equal(_srcTodoItem.Rank, _resultFields.Rank);
        }
    }
}
