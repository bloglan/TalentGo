using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 资格审查异常。
    /// </summary>
    [Serializable]
    public class FileReviewException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public FileReviewException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public FileReviewException(string message) : base(message) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public FileReviewException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected FileReviewException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
