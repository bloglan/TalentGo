using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    public interface ITalentGoSMSService
    {
        

        Task SendRealIdValidationMessageAsync(Person person);

        Task SendFileReviewMessageAsync(ApplicationForm form);

        Task SendApplicationFormAuditMessageAsync(ApplicationForm form);
    }
}
