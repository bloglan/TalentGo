﻿using System;
using System.Collections.Generic;

namespace TalentGo.Recruitment
{
    public class CommitEnrollmentException : Exception
    {
        List<string> archiveRequirementErrMsg;
        public CommitEnrollmentException(string Message, List<string> ArchiveRequirementErrMsg):base(Message)
        {
            archiveRequirementErrMsg = ArchiveRequirementErrMsg;
        }

        public List<string> ArchiveRequirementErrMsg
        {
            get
            {
                return this.archiveRequirementErrMsg;
            }
        }
    }
}