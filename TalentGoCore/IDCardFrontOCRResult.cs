using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 身份证正面信息。
    /// </summary>
    public class IDCardFrontOCRResult
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string SexString { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        public string Nationality { get; set; }

        /// <summary>
        /// 出生日期字符串
        /// </summary>
        public DateTime DateOfBirth { get; set; }
        
        /// <summary>
        /// 住址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 身份证号码。
        /// </summary>
        public string IDCardNumber { get; set; }
    }
}
