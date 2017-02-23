using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentGo.Recruitment;

namespace TalentGo.EntityFramework
{
    public class CollegeSuggestionProvider : ICollegeSuggestionProvider
    {
        TalentGoDbContext db;

        public CollegeSuggestionProvider(TalentGoDbContext DbContext)
        {
            this.db = DbContext;
        }

        public IEnumerable<string> SuggestCollegeName(string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return new string[0];

            var result = this.db.Database.SqlQuery<string>("SELECT TOP(20) Name FROM dbo.College WHERE Name LIKE N'%' + @p0 + '%'", Input);
            return result;
        }
    }
}
