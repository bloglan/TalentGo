using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 
    /// </summary>
    public static class CandidateExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="personId"></param>
        /// <returns></returns>
        public static IQueryable<Candidate> AvailableForUser(this IQueryable<Candidate> source, Guid personId)
        {
            return source.Where(c => c.Plan.WhenPublished.HasValue && c.PersonId == personId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<Candidate> AttendanceConfirmed(this IQueryable<Candidate> source)
        {
            return source.Where(c => c.Attendance.Value);
        }

        /// <summary>
        /// 从指定的招聘计划中，将已通过审核报名表导入为考试候选人。
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="examPlan"></param>
        /// <param name="recruitmentPlan"></param>
        /// <returns></returns>
        public static async Task ImportFromRecruitmentPlanAsync(this CandidateManager manager, ExaminationPlan examPlan, RecruitmentPlan recruitmentPlan)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            if (examPlan == null)
            {
                throw new ArgumentNullException(nameof(examPlan));
            }

            if (recruitmentPlan == null)
            {
                throw new ArgumentNullException(nameof(recruitmentPlan));
            }

            if (!recruitmentPlan.WhenAuditCommited.HasValue)
                throw new InvalidOperationException("审核尚未提交的招聘计划不能执行导入。");

            foreach(var job in recruitmentPlan.Jobs)
            {
                await ImportFromJobAsync(manager, examPlan, job);
            }
        }

        /// <summary>
        /// 从指定的职位所关联的已通过审核的报名表导入考试候选人。
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="examPlan"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        public static async Task ImportFromJobAsync(this CandidateManager manager, ExaminationPlan examPlan, Job job)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            if (examPlan == null)
            {
                throw new ArgumentNullException(nameof(examPlan));
            }

            if (job == null)
            {
                throw new ArgumentNullException(nameof(job));
            }

            foreach(var form in job.ApplicationForms.Approved())
            {
                await manager.CreateAsync(new Candidate(examPlan, form.Person));
            }
        }

        /// <summary>
        /// 从指定的考试计划关联的考试候选人中，依据指定的条件导入考试候选人。
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static async Task ImportFromExaminationPlan(this CandidateManager manager, ExaminationPlan target, ExaminationPlan source, Func<Candidate, bool> predicate)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.Equals(target))
                throw new InvalidOperationException("目标和源不能为同一个考试计划。");

            foreach(var candidate in source.Candidates.Where(predicate))
            {
                await manager.CreateAsync(new Candidate(target, candidate.Person));
            }
        }
    }
}
