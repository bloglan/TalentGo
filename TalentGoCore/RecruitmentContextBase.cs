using System.Security.Principal;

namespace TalentGo
{
    /// <summary>
    /// 封装与招聘业务有关信息的上下文。
    /// </summary>
    public abstract class RecruitmentContextBase
    {
        /// <summary>
        /// 获取该上下文的登陆用户。
        /// </summary>
        public abstract IPrincipal LoginUser { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract int? TargetUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public abstract int? SelectedPlanId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public abstract int? CurrentEnrollmentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual void ClearAll()
        {
            this.TargetUserId = null;
            this.SelectedPlanId = null;
            this.CurrentEnrollmentId = null;
        }

    }
}
