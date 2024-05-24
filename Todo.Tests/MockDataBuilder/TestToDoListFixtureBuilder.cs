using AutoFixture;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using Todo.Data.Entities;
using Todo.Tests.CustomFixture;

namespace Todo.Tests.MockDataBuilder
{
    public class TestToDoListFixtureBuilder
    {
        private readonly IFixture _fixture;
        private readonly string _title;
        private readonly IdentityUser _owner;
        private readonly List<TodoItem> _items= new List<TodoItem>();

        public TestToDoListFixtureBuilder(string title)
        {
            _fixture = new Fixture()
                                   .Customize(new IdentityUserFixture())
                                   .Customize(new TodoItemFixture());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                              .ToList()
                              .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _owner= _fixture.Create<IdentityUser>(); ;
            title = title ?? string.Empty;
        }      

        public TestToDoListFixtureBuilder WithItem()
        {         
           _items.AddRange(_fixture.CreateMany<TodoItem>(1).ToList());
            return this;
        }
       
        public TodoList Build()
        {
            var todoList = _fixture.Build<TodoList>()
               .With(temp => temp.Owner, _owner)
               .With(temp => temp.Title, _title)
               .With(temp => temp.Items, _items)
               .Create();

            todoList.Items = todoList.Items.Select(item =>
            {
                item.TodoList.Items = item.TodoList.Items.Select(innerItem =>
                {
                    innerItem.TodoListId = todoList.TodoListId;
                    return innerItem;
                }).ToList();
                item.TodoListId = todoList.TodoListId;
                item.TodoList.TodoListId = todoList.TodoListId;
                return item;
            }).ToList();

            return todoList;
        }
    }
}
