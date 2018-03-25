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
    public class ExamineeManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        public ExamineeManager(IExamineeStore store)
        {
            this.Store = store;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual IExamineeStore Store { get; set; }

        /// <summary>
        /// 获取应试人列表。
        /// </summary>
        public virtual IQueryable<Examinee> Examinees => this.Store.Examinees;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="examinee"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(Examinee examinee)
        {
            if (examinee == null)
            {
                throw new ArgumentNullException(nameof(examinee));
            }

            await this.Store.CreateAsync(examinee);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="examinee"></param>
        /// <param name="Attended"></param>
        /// <returns></returns>
        public virtual async Task ConfirmAttendanceAsync(Examinee examinee, bool Attended)
        {
            if (examinee == null)
            {
                throw new ArgumentNullException(nameof(examinee));
            }

            if (examinee.Subject.Plan.AttendanceConfirmationExpiresAt < DateTime.Now)
                throw new InvalidOperationException("截止日期之后不再受理声明");

            examinee.AttendanceConfirmed = Attended;
            examinee.WhenAttendanceConfirmed = DateTime.Now;

            await this.Store.UpdateAsync(examinee);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="examinee"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Examinee examinee)
        {
            
            await this.Store.UpdateAsync(examinee);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="examinee"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(Examinee examinee)
        {
            await this.Store.DeleteAsync(examinee);
        }
    }
}
