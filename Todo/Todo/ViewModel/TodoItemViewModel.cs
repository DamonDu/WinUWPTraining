using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.ViewModel
{
    class TodoItemViewModel
    {
        private ObservableCollection<DataModel.TodoListItem> allItems = new ObservableCollection<DataModel.TodoListItem>();
        public ObservableCollection<DataModel.TodoListItem> AllItems { get { return this.allItems; } }
    }
}
