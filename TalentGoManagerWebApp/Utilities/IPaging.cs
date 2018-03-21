using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGoManagerWebApp.Utilities
{
    public interface IPaging
    {
        int PageIndex { get; set; }

        int PageSize { get; set; }

        int AllCount { get; set; }
    }
}
