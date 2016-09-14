using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Backend
{
    public interface ISMSMessageStore
    {
        IQueryable<SMSMessageBag> SMSMessages { get; }

        Task CreateAsync(SMSMessageBag message);

        Task RemoveAsync(SMSMessageBag message);
    }
}