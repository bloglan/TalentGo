using System.Collections.Generic;
using TalentGo.Recruitment;

namespace TalentGoWebApp.Areas.Mgmt.Models
{
	public class BaseDataViewModel
    {
        public IEnumerable<ArchiveCategory> ArchiveCategoryList { get; set; }
    }
}