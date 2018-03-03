using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TalentGo.Mvc
{
    /// <summary>
    /// Mvc Html helpers.
    /// </summary>
    public static class MvcHtmlHelpers
    {
        /// <summary>
        /// Wite Description
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="self"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString DescriptionFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            var description = metadata.Description;

            if (string.IsNullOrWhiteSpace(description))
                return MvcHtmlString.Empty;

            return MvcHtmlString.Create(string.Format(@"<span class=""help-block"">{0}</span>", description));
        }
    }
}
