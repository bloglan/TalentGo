namespace TalentGo
{
    /// <summary>
    /// 表示一个考场。
    /// </summary>
    public abstract class ExaminationRoom
    {
        /// <summary>
        /// 考场标识符。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 考场地址。
        /// </summary>
        public string Address { get; set; }
    }
}
