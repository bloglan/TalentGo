namespace TalentGo.Recruitment
{
    /// <summary>
    /// 表示一个考试计划。
    /// </summary>
    public class ExaminationPlan
    {
        /// <summary>
        /// 考试计划Id。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 关联的招聘计划。
        /// </summary>
        public int RecruitmentPlanId { get; set; }
    }
}
