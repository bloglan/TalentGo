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
        public int PlanId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Person Person { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ExaminationPlan Plan { get; set; }

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
        public virtual string AdmissionNumber { get; set; }
    }
}
