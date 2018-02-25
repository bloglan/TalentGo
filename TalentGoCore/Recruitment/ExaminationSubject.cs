using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TalentGo.Recruitment
{
    /// <summary>
    /// 表示一个考试科目。
    /// </summary>
    public class ExaminationSubject
    {
        internal ExaminationSubject()
        {
            UsedExaminationRooms = new HashSet<ExaminationRoom>();
        }

        public ExaminationSubject(ExaminationPlan plan)
            : this()
        {
            ExaminationPlan = plan;
            ExaminationPlanId = plan.Id;
        }
        /// <summary>
        /// 考试科目标识符。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 关联考试计划。
        /// </summary>
        public int ExaminationPlanId { get; protected set; }

        [ForeignKey(nameof(ExaminationPlanId))]
        public virtual ExaminationPlan ExaminationPlan { get; protected set; }

        /// <summary>
        /// 考试科目。
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 考试开始时间。
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 考试结束时间。
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 使用的考场。
        /// </summary>
        public virtual ICollection<ExaminationRoom> UsedExaminationRooms { get; set; }
    }
}
