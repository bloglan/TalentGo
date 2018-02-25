namespace TalentGo.Backend
{
    /// <summary>
    /// 表示一个短信消息。
    /// </summary>
    public class SMSMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MessageBody { get; set; }
    }
}
