using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 身份证背面信息。
    /// </summary>
    public class IDCardBackOCRResult
    {
        /// <summary>
        /// 签发机关。
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 签发日期字符串。
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// 过期日期字符串。
        /// </summary>
        public DateTime? ExpiresDate { get; set; }
    }
}
