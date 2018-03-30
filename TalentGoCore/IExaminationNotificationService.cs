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
    public interface IExaminationNotificationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        Task NotifyPlanPublishedAsync(ExaminationPlan plan);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        Task NotifyAdmissionTicketReleasedAsync(ExaminationPlan plan);
    }
}
