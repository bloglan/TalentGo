﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 
    /// </summary>
    public class ExaminationManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Store"></param>
        public ExaminationManager(IExaminationStore Store)
        {
            this.Store = Store;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual IExaminationStore Store { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual IQueryable<Examination> Exams => this.Store.Exams;

        /// <summary>
        /// Find plan by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Examination> FindByIdAsync(int id)
        {
            return await this.Store.FindByIdAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(Examination exam)
        {
            if (exam == null)
            {
                throw new ArgumentNullException(nameof(exam));
            }

            if (exam.AttendanceConfirmationExpiresAt < DateTime.Now)
                throw new ArgumentException("确认参加考试的截止时间早于当前时间。");

            await this.Store.CreateAsync(exam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Examination exam)
        {
            if (exam == null)
            {
                throw new ArgumentNullException(nameof(exam));
            }
            if (exam.WhenPublished.HasValue)
                throw new InvalidOperationException("Can not update plan if published.");

            exam.WhenChanged = DateTime.Now;

            await this.Store.UpdateAsync(exam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(Examination exam)
        {
            if (exam == null)
            {
                throw new ArgumentNullException(nameof(exam));
            }
            if (exam.WhenPublished.HasValue)
                throw new InvalidOperationException("Cannot delete plan if published.");

            await this.Store.DeleteAsync(exam);
        }

        /// <summary>
        /// 发布考试。
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        public virtual async Task PublishAsync(Examination exam)
        {
            if (exam == null)
            {
                throw new ArgumentNullException(nameof(exam));
            }

            if (exam.AttendanceConfirmationExpiresAt < DateTime.Now)
                throw new ArgumentException("确认参加考试的截止时间早于当前时间。");

            exam.WhenPublished = DateTime.Now;

            await this.Store.UpdateAsync(exam);
        }


    }
}
