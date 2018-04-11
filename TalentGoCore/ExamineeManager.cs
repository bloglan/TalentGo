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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
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
