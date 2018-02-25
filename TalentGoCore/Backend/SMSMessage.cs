namespace TalentGo.Backend
{
    /// <summary>
    /// 表示一个短信消息。
    /// </summary>
    public class SMSMessage
    {
        public int Id { get; set; }

        public string To { get; set; }

        public string MessageBody { get; set; }
    }
}
