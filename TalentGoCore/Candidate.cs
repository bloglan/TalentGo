using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 考试候选人
    /// </summary>
    public class Candidate
    {
        /// <summary>
        /// 
        /// </summary>
        protected Candidate()
        {

        }

        /// <summary>
        /// 从用户创建考试候选人。
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="person"></param>
        /// <param name="applyFor"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Candidate(ExaminationPlan plan, Person person, string applyFor = null)
            : this()
        {
            this.Plan = plan ?? throw new ArgumentNullException(nameof(plan));
            this.ExamId = plan.Id;
            if (this.Plan.WhenPublished.HasValue)
                throw new InvalidOperationException("已发布的考试不能创建候选人。");
            this.Person = person ?? throw new ArgumentNullException(nameof(person));
            this.PersonId = person.Id;
            this.ApplyFor = applyFor;
        }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [ForeignKey(nameof(Person))]
        public Guid PersonId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [ForeignKey(nameof(Plan))]
        public int ExamId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Person Person { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ExaminationPlan Plan { get; set; }

        /// <summary>
        /// 申请职位。
        /// </summary>
        public virtual string ApplyFor { get; set; }

        /// <summary>
        /// 是否参加考试
        /// </summary>
        public bool? Attendance { get; internal set; }

        /// <summary>
        /// 确认时间。
        /// </summary>
        public DateTime? WhenConfirmed { get; internal set; }

        /// <summary>
        /// 准考证号。
        /// </summary>
        public virtual string AdmissionNumber { get; internal set; }

        /// <summary>
        /// 考场号
        /// </summary>
        public virtual string Room { get; internal set; }

        /// <summary>
        /// 座位号。
        /// </summary>
        public virtual string Seat { get; internal set; }
    }
}
