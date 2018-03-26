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
        /// <summary>
        /// 
        /// </summary>
        protected ExaminationPlan()
        {
            this.WhenCreated = DateTime.Now;
            this.WhenChanged = DateTime.Now;
            this.Candidates = new HashSet<Candidate>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="attendanceConfirmationExpiresAt"></param>
        public ExaminationPlan(string title, DateTime attendanceConfirmationExpiresAt)
            : this()
        {
            this.Title = title;
            this.AttendanceConfirmationExpiresAt = attendanceConfirmationExpiresAt;
        }

        
        /// <summary>
        /// 考试计划Id。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 标题。
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime WhenCreated { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime WhenChanged { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? WhenPublished { get; internal set; }

        /// <summary>
        /// 确认参考截止日期。
        /// </summary>
        public virtual DateTime AttendanceConfirmationExpiresAt { get; set; }

        /// <summary>
        /// Gets subjects of this plan.
        /// </summary>
        public virtual ICollection<ExaminationSubject> Subjects { get; protected set; }
        
        /// <summary>
        /// 考试候选人。
        /// </summary>
        public virtual ICollection<Candidate> Candidates { get; protected set; }
    }
}
