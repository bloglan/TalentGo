namespace TalentGo.Backend
{
    /// <summary>
    /// 邮件消息。
    /// </summary>
    public class EmailMessage
    {
        /// <summary>
        /// 收件人。
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// 标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 邮件正文。
        /// </summary>
        public string MessageBody { get; set; }
    }
}
