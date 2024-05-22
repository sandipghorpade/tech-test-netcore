using Todo.Data.Entities;
using Todo.Models.TodoItems;

namespace Todo.EntityModelMappers.TodoItems
{
    public static class TodoItemSummaryViewmodelFactory
    {
        public static TodoItemSummaryViewmodel Create(TodoItem ti)
        {
            return new TodoItemSummaryViewmodel(ti.TodoItemId, ti.Title, ti.IsDone, ti.Rank, UserSummaryViewmodelFactory.Create(ti.ResponsibleParty), ti.Importance);
        }
    }
}