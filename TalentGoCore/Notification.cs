using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 通知公告。
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// protected ctor.
        /// </summary>
        protected Notification()
        {
            this.WhenCreated = DateTime.Now;
        }

        /// <summary>
        /// Create a notification that attach to the plan.
        /// </summary>
        /// <param name="plan"></param>
        public Notification(RecruitmentPlan plan)
        {
            this.Plan = plan;
            this.PlanId = plan.Id;
        }

        /// <summary>
        /// 通知公告的Id.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// PlanId.
        /// </summary>
        [ForeignKey(nameof(Plan))]
        public int PlanId { get; protected set; }

        /// <summary>
        /// Plan
        /// </summary>
        public RecruitmentPlan Plan { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// WhenCreated.
        /// </summary>
        public DateTime WhenCreated { get; protected set; }

    }
}
