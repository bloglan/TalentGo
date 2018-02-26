using System.Collections.Generic;
using TalentGo;

namespace TalentGoWebApp.Areas.Mgmt.Models
{
    public class BaseDataViewModel
    {
        public IEnumerable<ArchiveCategory> ArchiveCategoryList { get; set; }
    }
}