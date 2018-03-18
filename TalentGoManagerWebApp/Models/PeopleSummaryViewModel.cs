using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalentGoManagerWebApp.Models
{
    public class PeopleSummaryViewModel
    {
        public int AllUserCount { get; set; }

        public int PendingRealIdValidationCount { get; set; }

        public int RealdIdAcceptedCount { get; set; }
    }
}