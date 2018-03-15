using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    public static class PeopleExtensions
    {
        public static IQueryable<Person> PendingRealIdValid(this IQueryable<Person> source)
        {
            return source.Where(p => p.WhenRealIdCommited.HasValue && !p.WhenRealIdValid.HasValue);
        }
    }
}
