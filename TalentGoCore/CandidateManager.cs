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
        /// <param name="personId"></param>
        /// <param name="examId"></param>
        /// <returns></returns>
        public async Task<Candidate> FindByIdAsync(Guid personId, int examId)
        {
            return await this.Store.FindByIdAsync(personId, examId);
        }

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

            if (!candidate.Plan.AttendanceConfirmationExpiresAt.HasValue)
                throw new NotSupportedException();

            if (candidate.Plan.AttendanceConfirmationExpiresAt.Value < DateTime.Now)
                throw new InvalidOperationException("声明截止日期后不能再声明。");

            candidate.WhenConfirmed = DateTime.Now;
            candidate.Attendance = Attend;
            await this.Store.UpdateAsync(candidate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="ticketNumber"></param>
        /// <returns></returns>
        public async Task SetTicketNumberAsync(Candidate candidate, string ticketNumber)
        {
            if (candidate == null)
            {
                throw new ArgumentNullException(nameof(candidate));
            }

            var examId = candidate.ExamId;
            if (!string.IsNullOrEmpty(ticketNumber))
            {
                if (this.Candidates.Any(c => c.ExamId == examId && c.AdmissionNumber == ticketNumber))
                    throw new InvalidOperationException("准考证号出现重复。");
            }
            candidate.AdmissionNumber = string.IsNullOrEmpty(ticketNumber) ? null : ticketNumber;
            await this.Store.UpdateAsync(candidate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="room"></param>
        /// <param name="seat"></param>
        /// <returns></returns>
        public async Task SetRoomSeatAsync(Candidate candidate, string room, string seat)
        {
            if (candidate == null)
                throw new ArgumentNullException(nameof(candidate));

            var examId = candidate.ExamId;
            if (!string.IsNullOrEmpty(room) && !string.IsNullOrEmpty(seat))
            {
                if (this.Candidates.Any(c => c.ExamId == examId && c.Room == room && c.Seat == seat))
                    throw new InvalidOperationException("考场和座位出现重复。");

            }

            candidate.Room = string.IsNullOrEmpty(room) ? null : room;
            candidate.Seat = string.IsNullOrEmpty(seat) ? null : seat;
            await this.Store.UpdateAsync(candidate);
        }
    }
}
