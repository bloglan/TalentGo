using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalentGoManagerWebApp.Utilities
{
    public class Paging : IPaging
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int AllCount { get; set; }
    }
}