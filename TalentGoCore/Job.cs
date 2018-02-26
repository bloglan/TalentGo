using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 表示要招聘的职位。
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Plan Id.
        /// </summary>
        [ForeignKey(nameof(Plan))]
        public int PlanId { get; protected set; }

        /// <summary>
        /// The job belongs to plan.
        /// </summary>
        public RecruitmentPlan Plan { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Work Direction.
        /// </summary>
        public string WorkDirection { get; set; }

        /// <summary>
        /// Work Location.
        /// </summary>
        public string WorkLocation { get; set; }

        /// <summary>
        /// Requirements.
        /// </summary>
        public string Requirements { get; set; }
    }
}
