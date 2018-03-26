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
    /// 表示应试人。
    /// </summary>
    public class Examinee
    {
        /// <summary>
        /// 
        /// </summary>
        protected Examinee()
        {
        }

        /// <summary>
        /// 考试计划Id.
        /// </summary>
        [Key]
        [Column(Order = 0)]
        public virtual int PlanId { get; set; }

        /// <summary>
        /// 科目Id
        /// </summary>
        [Key]
        [Column(Order = 1)]
        public virtual int SubjectId { get; set; }

        /// <summary>
        /// 用户Id。
        /// </summary>
        [Key]
        [Column(Order = 2)]
        public virtual Guid PersonId { get; set; }

        /// <summary>
        /// 科目。
        /// </summary>
        [ForeignKey(nameof(PlanId) + "," + nameof(SubjectId))]
        public virtual ExaminationSubject Subject { get; set; }

        /// <summary>
        /// 用户。
        /// </summary>
        [ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; }

        /// <summary>
        /// 考场
        /// </summary>
        public virtual string Room { get; set; }

        /// <summary>
        /// 座位
        /// </summary>
        public virtual string Seat { get; set; }

        /// <summary>
        /// 计分。
        /// </summary>
        public virtual decimal? Score { get; set; }
    }
}
