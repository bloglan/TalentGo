using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 候选人管理器
    /// </summary>
    public class CandidateManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        public CandidateManager(ICandidateStore store)
        {
            this.Store = store;
        }

        /// <summary>
        /// 
        /// </summary>
        protected ICandidateStore Store { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<Candidate> Candidates => this.Store.Candidates;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public async Task CreateAsync(Candidate candidate)
        {
            await this.Store.CreateAsync(candidate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Candidate candidate)
        {
            await this.Store.DeleteAsync(candidate);
        }

        /// <summary>
        /// 确认是否参加考试。
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="Attend"></param>
        /// <returns></returns>
        public async Task ConfirmAttendance(Candidate candidate, bool Attend)
        {
            if (candidate == null)
            {
                throw new ArgumentNullException(nameof(candidate));
            }

            if (candidate.Plan.AttendanceConfirmationExpiresAt < DateTime.Now)
                throw new InvalidOperationException("声明截止日期后不能再声明。");

            candidate.WhenConfirmed = DateTime.Now;
            candidate.Attendance = Attend;
            await this.Store.UpdateAsync(candidate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="admissionNumber"></param>
        /// <param name="room"></param>
        /// <param name="seat"></param>
        /// <returns></returns>
        public async Task AssignAdmissionTicket(Candidate candidate, string admissionNumber, string room, string seat)
        {
            if (candidate == null)
            {
                throw new ArgumentNullException(nameof(candidate));
            }
            if (string.IsNullOrEmpty(admissionNumber))
                throw new ArgumentNullException(nameof(admissionNumber));
            var examId = candidate.ExamId;
            if (this.Candidates.Any(c => c.ExamId == examId && c.AdmissionNumber == admissionNumber))
                throw new ArgumentException("准考证重复。");
            if (this.Candidates.Any(c => c.ExamId == examId && c.Room == room && c.Seat == seat))
                throw new ArgumentException("考场座位重复。");

            candidate.AdmissionNumber = admissionNumber;
            candidate.Room = room;
            candidate.Seat = seat;

            await this.Store.UpdateAsync(candidate);
        }

        
    }
}
