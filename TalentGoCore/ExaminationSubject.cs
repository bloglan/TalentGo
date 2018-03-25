using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TalentGo
{
    /// <summary>
    /// 表示一个考试科目。
    /// </summary>
    public class ExaminationSubject
    {
        /// <summary>
        /// 
        /// </summary>
        public ExaminationSubject()
        {
        }

        /// <summary>
        /// 考试科目标识符。
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }

        /// <summary>
        /// 关联考试计划。
        /// </summary>
        [Key]
        [Column(Order = 1)]
        public int PlanId { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(PlanId))]
        public virtual ExaminationPlan Plan { get; protected set; }

        /// <summary>
        /// 考试科目。
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 考试开始时间。
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 考试结束时间。
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 考试地点。
        /// </summary>
        public virtual string Address { get; set; }

    }
}
