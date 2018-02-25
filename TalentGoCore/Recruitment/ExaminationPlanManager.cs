using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{

    public class ExaminationPlanManager
    {
        IExaminationPlanStore store;

        public ExaminationPlanManager(IExaminationPlanStore Store)
        {
            this.store = Store;
        }


    }
}
