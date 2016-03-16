using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LowStockApp.Classes
{
    public class QueryResult
    {
        public QueryResult() { }
        public int TotalResults = 0;
        public List<Dictionary<string, object>> Results { get; set; }
        public List<CustomScriptColumn> Columns { get; set; }
    }

    public class CustomScriptColumn
    {
        public CustomScriptColumn() { }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}