namespace TalentGo.Recruitment
{
    /// <summary>
    /// 考场管理器
    /// </summary>
    public class ExaminationRoomManager
    {
        IExaminationRoomStore store;

        public ExaminationRoomManager(IExaminationRoomStore Store)
        {
            this.store = Store;
        }


    }
}
