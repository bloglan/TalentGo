using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    /// <summary>
    /// Supports provide college name suggestion.
    /// </summary>
    public interface ICollegeSuggestionProvider
    {
        /// <summary>
        /// Get suggest college name list via input.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        IEnumerable<string> SuggestCollegeName(string Input);
    }
}
