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
    public interface IRecruitmentPlanNotificationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        Task NotifyAuditCompleteAsync(RecruitmentPlan plan);
    }
}
