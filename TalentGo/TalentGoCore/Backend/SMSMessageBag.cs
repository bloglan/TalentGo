using System;
using System.ComponentModel.DataAnnotations;

namespace TalentGo.Backend
{
    public class SMSMessageBag
    {
        [Key]
        public int Id { get; set; }

        public DateTime? SentTime { get; set; }

        public virtual SMSMessage Message { get; set; }
    }
}
