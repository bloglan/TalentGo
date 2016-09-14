using System.Security.Principal;

namespace TalentGo.Core
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

        public abstract int? TargetUserId { get; set; }

        public abstract int? SelectedPlanId { get; set; }

        public abstract int? CurrentEnrollmentId { get; set; }

        public virtual void ClearAll()
        {
            this.TargetUserId = null;
            this.SelectedPlanId = null;
            this.CurrentEnrollmentId = null;
        }

    }
}
