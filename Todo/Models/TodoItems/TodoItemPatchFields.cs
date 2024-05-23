using System.ComponentModel.DataAnnotations;
using Todo.Data.Entities;

namespace Todo.Models.TodoItems
{
    public class TodoItemPatchFields
    {
        public int TodoListId { get; set; }
        public string Title { get; set; }
        public string TodoListTitle { get; set; }
        public int TodoItemId { get; set; }
        public bool IsDone { get; set; }
        [Display(Name = "Responsible Party")]
        public string ResponsiblePartyId { get; set; }
        public int? Rank { get; set; }
        public Importance Importance { get; set; }

        public TodoItemPatchFields() { }

        public TodoItemPatchFields(int todoListId, string todoListTitle,
                                  int todoItemId, string title,
                                  bool isDone, string responsiblePartyId,
                                  int? rank, Importance importance)
        {
            TodoListId = todoListId;
            TodoListTitle = todoListTitle;
            TodoItemId = todoItemId;
            Title = title;
            IsDone = isDone;
            ResponsiblePartyId = responsiblePartyId;
            Importance = importance;
            Rank = rank;
        }
    
    }
}
