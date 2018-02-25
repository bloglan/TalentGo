using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    /// <summary>
    /// 提供对考场的数据存储访问能力。
    /// </summary>
    public interface IExaminationRoomStore : IDisposable
    {
        /// <summary>
        /// 获取考场数据源。
        /// </summary>
        IQueryable<ExaminationRoom> ExaminationRooms { get; }

        /// <summary>
        /// 创建考场。
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        Task CreateAsync(ExaminationRoom room);

        /// <summary>
        /// 更新考场信息。
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        Task UpdateAsync(ExaminationRoom room);

        /// <summary>
        /// 删除考场信息。
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        Task DeleteAsync(ExaminationRoom room);
    }
}
