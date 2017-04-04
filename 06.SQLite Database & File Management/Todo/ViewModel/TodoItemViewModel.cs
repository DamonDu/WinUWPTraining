using System.Collections.ObjectModel;
using System.Linq;
using Todo.Models;

namespace Todo.ViewModels
{
    public class TodoItemViewModel
    {
        private ObservableCollection<TodoItem> Items = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<TodoItem> AllItems { get { return this.Items; } }

        private TodoItem selectedItem = default(TodoItem);
        public TodoItem SelectedItem { get; set; }

        public TodoItem NewestItem { get; set; }

        public TodoItemViewModel() { }

        public void AddTodoItem(Models.TodoItem todo)
        {
            Items.Add(todo);
        }

        public void DeleteTodoItem(string id)
        {
            Items.Remove(this.Items.SingleOrDefault(i => i.Id == id));
            selectedItem = null;
        }

        public void UpdateTodoItem(Models.TodoItem OriginTodo, Models.TodoItem UpdateInfo)
        {
            int index = this.Items.IndexOf(OriginTodo);
            if (index >= 0 && index < this.Items.Count)
            {
                this.Items[index] = UpdateInfo;
            }
            this.selectedItem = null;
        }

        public Models.TodoItem GetItemById(string id)
        {
            return Items.FirstOrDefault(i => i.Id == id);
        }
    }
}
