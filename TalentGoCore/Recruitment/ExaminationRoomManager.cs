namespace TalentGo.Recruitment
{
    /// <summary>
    /// 考场管理器
    /// </summary>
    public class ExaminationRoomManager
    {
        IExaminationRoomStore store;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Store"></param>
        public ExaminationRoomManager(IExaminationRoomStore Store)
        {
            this.store = Store;
        }


    }
}
