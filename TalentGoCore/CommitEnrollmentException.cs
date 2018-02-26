using System;
using System.Collections.Generic;

namespace TalentGo
{
    /// <summary>
    /// 
    /// </summary>
    public class CommitEnrollmentException : Exception
    {
        List<string> archiveRequirementErrMsg;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="ArchiveRequirementErrMsg"></param>
        public CommitEnrollmentException(string Message, List<string> ArchiveRequirementErrMsg):base(Message)
        {
            archiveRequirementErrMsg = ArchiveRequirementErrMsg;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> ArchiveRequirementErrMsg
        {
            get
            {
                return this.archiveRequirementErrMsg;
            }
        }
    }
}
