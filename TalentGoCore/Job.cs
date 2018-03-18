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
    /// 表示要招聘的职位。
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Default ctor.
        /// </summary>
        public Job()
        {
            this.ApplicationForms = new HashSet<ApplicationForm>();
        }

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }

        /// <summary>
        /// Plan Id.
        /// </summary>
        [Key]
        [Column(Order = 1)]
        public int PlanId { get; protected set; }

        /// <summary>
        /// The job belongs to plan.
        /// </summary>
        [ForeignKey(nameof(PlanId))]
        public virtual RecruitmentPlan Plan { get; protected set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 计划招聘人数。
        /// </summary>
        public virtual int? ExpectRecruitCount { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Work Location.
        /// </summary>
        public string WorkLocation { get; set; }

        /// <summary>
        /// 学历要求。
        /// </summary>
        public string EducationBackgroundRequirement { get; set; }

        /// <summary>
        /// 学位要求。
        /// </summary>
        public string DegreeRequirement { get; set; }

        /// <summary>
        /// 专业要求。
        /// </summary>
        public string MajorRequirement { get; set; }

        /// <summary>
        /// 获取与此职位相关联的报名表。
        /// </summary>
        public virtual ICollection<ApplicationForm> ApplicationForms { get; protected set; }
    }
}
