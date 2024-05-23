using Todo.Data.Entities;
using Todo.Models.TodoItems;

namespace Todo.EntityModelMappers.TodoItems
{
    public class TodoItemPatchFieldsFactory
    {
        public static TodoItemPatchFields Create(TodoItem todoItem)
        {
            var todoList = todoItem.TodoList;
            return new TodoItemPatchFields(todoList.TodoListId, todoList.Title, todoItem.TodoItemId, todoItem.Title,
                todoItem.IsDone, todoItem.ResponsiblePartyId, todoItem.Rank, todoItem.Importance);
        }

        public static void Update(TodoItemPatchFields src, TodoItem dest)
        {
            dest.Title = src.Title;
            dest.IsDone = src.IsDone;
            dest.ResponsiblePartyId = src.ResponsiblePartyId;
            dest.Importance = src.Importance;
            dest.Rank = src.Rank;
        }
    }
}
