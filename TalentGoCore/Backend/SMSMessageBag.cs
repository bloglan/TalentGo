using System;
using System.ComponentModel.DataAnnotations;

namespace TalentGo.Backend
{
    public class SMSMessageBag
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? SentTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual SMSMessage Message { get; set; }
    }
}
