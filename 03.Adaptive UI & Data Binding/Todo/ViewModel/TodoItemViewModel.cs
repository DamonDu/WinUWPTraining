using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.DataModel;

namespace Todo.ViewModel
{
    class TodoItemViewModel
    {
        private ObservableCollection<TodoListItem> allItems = new ObservableCollection<TodoListItem>();
        public ObservableCollection<TodoListItem> AllItems { get { return this.allItems; } }

        private TodoListItem selectedItem = default(TodoListItem);
        public TodoListItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; } }

        public TodoItemViewModel()
        {
            this.allItems.Add(new TodoListItem("123", "123",DateTime.Today));
            this.allItems.Add(new TodoListItem("456", "456", DateTime.Today));
        }

        public void AddTodoItem(string title, string description, DateTime date)
        {
            TodoListItem newItem = new TodoListItem(title, description, date);
            this.allItems.Add(newItem);
            newItem.change_property();
        }

        public void RemoveTodoItem()
        {
            this.allItems.Remove(selectedItem);
            selectedItem.change_property();
            this.selectedItem = null;
        }

        public void UpdateTodoItem(string title, string description, DateTime date)
        {
            if(this.selectedItem != null)
            {
                TodoListItem changeItem = new TodoListItem(title, description, date);
                int index = allItems.IndexOf(selectedItem);
                this.allItems.Remove(selectedItem);
                this.allItems.Insert(index, changeItem);
                changeItem.change_property();
                this.selectedItem = null;
            }
        }
    }
}
