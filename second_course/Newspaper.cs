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
    public class Newspaper : INotifyPropertyChanged
    {
        private Boolean _i_is_read;
        public Boolean i_is_parsed;
        public Int32 id { get; set; }
        public Int32 i_thematic_id { get; set; }
        public String s_header { get; set; }
        public String s_description { get; set; }
        public Int32 i_source_id { get; set; }
        public String s_date { get; set; }
        public Boolean i_is_read {
            get
            {
                return _i_is_read;
            }
            set
            {
                _i_is_read = value;
                OnPropertyChanged();
            }
        }
        public String s_link { get; set; }
        public String s_full_text { get; set; }
        public String SourceName { get; set; }

        public Newspaper()
        {
            this.id = -1; // указывает, что id не был назначен
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
