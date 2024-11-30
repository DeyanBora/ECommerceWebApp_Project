using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebApp.Entities.Entities.Search
{
    public class SearchParameters
    {
        public string Query { get; set; } = string.Empty;
        public Dictionary<string, object> Filters { get; set; } = new();
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
