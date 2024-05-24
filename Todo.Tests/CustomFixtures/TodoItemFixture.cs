using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Todo.Data.Entities;

namespace Todo.Tests.CustomFixture
{
    public class TodoItemFixture : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<TodoItem>(composer =>
                composer.FromFactory(() =>
                {
                    var user = fixture.Create<IdentityUser>();
                    return new TodoItem(1, user.Id, "title", 1, Importance.High);
                }));
        }
    }
}
