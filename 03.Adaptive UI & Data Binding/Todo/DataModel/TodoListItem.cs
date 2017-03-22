using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Todo.DataModel
{
    public class TodoListItem : INotifyPropertyChanged
    {
        public string tid { get; private set; }
        public string title { get;  set; }
        public string description { get;  set; }
        public bool ifCompleted { get;  set; }
        DateTime _date;
        public DateTime date
        {
            get { return _date; }
            set
            {
                _date = value;
                //change_property();
            }
        }

        public TodoListItem(string title, string description, DateTime date)
        {
            this.tid = Guid.NewGuid().ToString();
            this.title = title;
            this.description = description;
            this.ifCompleted = false;
            this.date = date;
            //change_property();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void change_property([CallerMemberName] string property_name = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }

    }
}
