using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using second_course.Annotations;

namespace second_course
{
    class TableColumn : INotifyPropertyChanged
    {
        private Boolean _selected;

        public String name { get; set; }

        public Boolean selected
        {
            get { return _selected; }
            set
            {
                OnPropertyChanged();
                _selected = value;
            }
        }

        public TableColumn()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
