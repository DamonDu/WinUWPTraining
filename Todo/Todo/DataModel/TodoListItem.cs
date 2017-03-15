using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.DataModel
{
    class TodoListItem
    {
        private string tid;
        public string title { get;  set; }
        public string description { get;  set; }
        public bool ifCompleted { get;  set; }
        public DateTime date { get;  set; }

        public TodoListItem(string title, string description, DateTime date)
        {
            this.tid = Guid.NewGuid().ToString();
            this.title = title;
            this.description = description;
            this.ifCompleted = false;
            this.date = date;
        }
    }
}
