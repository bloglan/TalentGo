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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
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
        /// <param name="address"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExaminationPlan(string title, string address)
            : this()
        {
            this.Title = title;
            this.Address = address;
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
        public virtual DateTime? AttendanceConfirmationExpiresAt { get; set; }

        /// <summary>
        /// 考试地点。
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// 考试须知。
        /// </summary>
        public virtual string Notes { get; set; }

        /// <summary>
        /// 发放准考证时间。
        /// </summary>
        public virtual DateTime? WhenAdmissionTicketReleased { get; internal set; }

        /// <summary>
        /// 考试科目列表。
        /// </summary>
        public virtual ICollection<ExaminationSubject> Subjects { get; protected set; }

        /// <summary>
        /// 与此考试计划关联的候选人。
        /// </summary>
        public virtual ICollection<Candidate> Candidates { get; protected set; }
    }
}
