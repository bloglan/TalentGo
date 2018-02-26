using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 
    /// </summary>
    public class ExaminationPlanManager
    {
        IExaminationPlanStore store;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Store"></param>
        public ExaminationPlanManager(IExaminationPlanStore Store)
        {
            this.store = Store;
        }


    }
}
