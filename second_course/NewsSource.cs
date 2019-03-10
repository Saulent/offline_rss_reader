using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace second_course
{
    public class NewsSource
    {
        public Int32 id { get; set; }
        public String s_name { get; set; }
        public String s_rss_link { get; set; }
        public Boolean i_is_chosen { get; set; }


        public NewsSource() { }

        public NewsSource(String header, String link, Boolean isSelected)
        {
            this.s_name = header;
            this.s_rss_link = link;
            this.i_is_chosen = isSelected;
        }
    }
}
