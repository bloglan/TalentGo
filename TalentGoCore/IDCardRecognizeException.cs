using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 身份证识别异常。
    /// </summary>
    [Serializable]
    public class IDCardRecognizeException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public IDCardRecognizeException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public IDCardRecognizeException(string message) : base(message) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public IDCardRecognizeException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected IDCardRecognizeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
