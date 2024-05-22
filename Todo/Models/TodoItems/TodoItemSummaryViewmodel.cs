using Todo.Data.Entities;

namespace Todo.Models.TodoItems
{
    public class TodoItemSummaryViewmodel
    {
        public int TodoItemId { get; }
        public string Title { get; }
        public UserSummaryViewmodel ResponsibleParty { get; }
        public bool IsDone { get; }
        public int? Rank { get; set; }
        public Importance Importance { get; }

        public TodoItemSummaryViewmodel(int todoItemId, string title, bool isDone, UserSummaryViewmodel responsibleParty, Importance importance)
        {
            TodoItemId = todoItemId;
            Title = title;
            IsDone = isDone;
            ResponsibleParty = responsibleParty;
            Importance = importance;
        }

        public TodoItemSummaryViewmodel(int todoItemId, string title,
                                        bool isDone, int? rank, 
                                        UserSummaryViewmodel responsibleParty, Importance importance)
                                        :this(todoItemId, title, isDone, responsibleParty, importance)
        {
            Rank = rank;
        }
    }
}