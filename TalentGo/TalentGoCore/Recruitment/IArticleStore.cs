using System.Linq;
using System.Threading.Tasks;
using TalentGo.Utilities;

namespace TalentGo.Recruitment
{
    public interface IArticleStore
    {
        IQueryable<Article> Articles { get; }

        Task CreateAsync(Article article);

        Task UpdateAsync(Article article);

        Task RemoveAsync(Article article);
    }
}
