using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace second_course
{
    static class Search
    {
        public static List<Newspaper> SearchByQuery(List<Newspaper> newspapers, String query)
        {
            if (query.Length == 0)
                return newspapers;

            query = query.ToLower();

            List<Newspaper> result = new List<Newspaper>();

            foreach (Newspaper newspaper in newspapers)
            {
                if (newspaper.s_header.ToLower().Contains(query) || newspaper.s_description.ToLower().Contains(query)) 
                {
                    result.Add(newspaper);
                }
            }
            
            return result;
        }
    }
}
