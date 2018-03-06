using System.Collections.Generic;
using TalentGo;

namespace TalentGoManagerWebApp.Models
{
    public class BaseDataViewModel
    {
        public IEnumerable<ArchiveCategory> ArchiveCategoryList { get; set; }
    }
}