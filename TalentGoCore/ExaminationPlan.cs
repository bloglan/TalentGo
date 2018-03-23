using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TalentGo
{
    /// <summary>
    /// 表示一个考试计划。
    /// </summary>
    public class ExaminationPlan
    {
        internal ExaminationPlan()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recruitmentPlan"></param>
        public ExaminationPlan(RecruitmentPlan recruitmentPlan)
        {
        }

        /// <summary>
        /// 考试计划Id。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime WhenCreated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime WhenChanted { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? WhenPublished { get; protected set; }

        /// <summary>
        /// Gets subjects of this plan.
        /// </summary>
        public virtual ICollection<ExaminationSubject> Subjects { get; protected set; }
        
    }
}
