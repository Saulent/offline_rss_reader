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
    class JoinTableElement : INotifyPropertyChanged
    {
        private string _firstTable;
        private string _firstField;
        private string _secondTable;
        private string _secondField;
        public JoinTableElementStorage OwnStorage { get; set; }
        public string FirstTable
        {
            get => _firstTable;
            set
            {
                _firstTable = value;
                OnPropertyChanged();
            }
        }
        public string FirstField
        {
            get => _firstField;
            set
            {
                _firstField = value;
                OnPropertyChanged();
            }
        }
        public string SecondTable
        {
            get => _secondTable;
            set
            {
                _secondTable = value;
                OnPropertyChanged();
            }
        }
        public string SecondField
        {
            get => _secondField;
            set
            {
                _secondField = value;
                OnPropertyChanged();
            }
        }

        public JoinTableElement()
        { }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class JoinTableElementStorage
    {
        public List<string> FirstTable { get; set; }
        public List<string> FirstField { get; set; }
        public List<string> SecondTable { get; set; }
        public List<string> SecondField { get; set; }

        public JoinTableElementStorage() { }
    }

    class JoinTableElementPack
    {
        public JoinTableElement Element { get; set; }
        public JoinTableElementStorage Storage { get; set; }

        public JoinTableElementPack() { }
    }
}
