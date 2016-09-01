using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TalentGo.EntityFramework;
using TalentGo.Recruitment;

namespace TalentGo.Utilities
{
	public class ArticleManager
	{
		TalentGoDbContext database;
		public ArticleManager(HttpContextBase httpContext)
		{
			this.database = TalentGoDbContext.FromContext(httpContext);
		}

		/// <summary>
		/// 获取有效的文章列表，该方法返回最多50条数据。并且以修改时间倒序排列。
		/// </summary>
		/// <param name="IsPublic">指示一个值，表示要取得适用于外网还是内网的文章。</param>
		/// <param name="plan">可以为null，则表示列表与招聘计划无关。若不为null，则显示与招聘计划关联的文章列表。忽略IsPublic参数。</param>
		/// <returns></returns>
		public async Task<IEnumerable<Article>> GetAvaiableArticles(bool IsPublic, RecruitmentPlan plan)
		{
			if (plan == null || plan.id == 0)
			{
				var articleSet = from article in this.database.Article
								 where (!article.IsPublic.HasValue || article.IsPublic.Value == IsPublic) && article.Visible && !article.RelatedPlan.HasValue
								 orderby article.WhenChanged descending
								 select article;
				return articleSet.Take(50);
			}
			else
			{
				var planArticleSet = from article in this.database.Article
									 where article.RelatedPlan == plan.id && article.Visible
									 orderby article.WhenChanged descending
									 select article;
				return planArticleSet.Take(50);
			}
		}

		public Article FindByID(int id)
		{
			return this.database.Article.SingleOrDefault(e => e.id == id);
		}

		public async Task CreateArticle(Article article)
		{
			article.WhenCreated = DateTime.Now;
			article.WhenChanged = DateTime.Now;

			this.database.Article.Add(article);

			await this.database.SaveChangesAsync();
		}

		public async Task UpdateArticle(Article article)
		{
			var selected = this.database.Article.SingleOrDefault(e => e.id == article.id);
			if (selected != null)
			{
				var selectedEntry = this.database.Entry<Article>(selected);
				selectedEntry.CurrentValues.SetValues(article);

				await this.database.SaveChangesAsync();
			}
		}

		public async Task RemoveArticleByID(int id)
		{
			var selected = this.FindByID(id);
			if (selected != null)
			{
				this.database.Article.Remove(selected);
				await this.database.SaveChangesAsync();
			}
		}

		/// <summary>
		/// 使用给定的关键字，页序号和每页项数取得按修改时间排序的文章列表。该方法除了返回指定页序号内的文章列表外，将返回总文章数以便计算分页。
		/// </summary>
		/// <param name="Keywords">关键字，可以为空或null，表示不使用关键字过滤。</param>
		/// <param name="PageIndex">从0开始的页号</param>
		/// <param name="PageSize">每页项数</param>
		/// <param name="ItemCount">返回一个总文章计数</param>
		/// <returns>返回指定页上的文章列表</returns>
		public IEnumerable<Article> GetArticles(string Keywords, int PageIndex, int PageSize, out int ItemCount)
		{
			if (string.IsNullOrEmpty(Keywords))
				Keywords = string.Empty;
			var articleSet = from article in this.database.Article
							where article.Title.Contains(Keywords)
							orderby article.WhenChanged descending
							select article;
			ItemCount = articleSet.Count();
			return articleSet.Skip(PageIndex * PageSize).Take(PageSize);
		}
	}
}
