﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TalentGo.Recruitment
{
    /// <summary>
    /// 表示一个考试计划。
    /// </summary>
    public class ExaminationPlan
    {
        internal ExaminationPlan()
        {

        }

        public ExaminationPlan(RecruitmentPlan recruitmentPlan)
        {
            this.RecruitmentPlan = recruitmentPlan;
            this.RecruitmentPlanId = recruitmentPlan.id;
        }
        /// <summary>
        /// 考试计划Id。
        /// </summary>
        public int Id { get; set; }

        public DateTime WhenCreated { get; set; }

        public DateTime WhenChanted { get; set; }

        public DateTime? WhenPublished { get; protected set; }

        /// <summary>
        /// 关联的招聘计划。
        /// </summary>
        public int RecruitmentPlanId { get; protected set; }

        /// <summary>
        /// 获取与此考试计划有关的招聘计划。
        /// </summary>
        [ForeignKey(nameof(RecruitmentPlanId))]
        public virtual RecruitmentPlan RecruitmentPlan { get; protected set; }

        /// <summary>
        /// Gets subjects of this plan.
        /// </summary>
        public virtual ICollection<ExaminationSubject> Subjects { get; protected set; }

        [NotMapped]
        public bool HasPublished { get { return this.WhenPublished.HasValue; } }


        internal virtual void Publish()
        {
            if (this.HasPublished)
                throw new InvalidOperationException("this plan has been published.");

            this.WhenPublished = DateTime.Now;
        }

        
    }
}
