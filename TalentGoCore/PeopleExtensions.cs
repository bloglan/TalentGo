using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 关于用户集合的扩展方法。
    /// </summary>
    public static class PeopleExtensions
    {
        /// <summary>
        /// 获取等待实名身份验证的用户集合。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<Person> PendingRealIdValid(this IQueryable<Person> source)
        {
            return source.Where(p => p.WhenRealIdCommited.HasValue && !p.WhenRealIdValid.HasValue);
        }
    }
}
