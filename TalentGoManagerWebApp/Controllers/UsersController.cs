using System;
using System.Linq;
using System.Web.Mvc;
using TalentGo.Identity;
using TalentGo.Linq;
using TalentGo;
using TalentGo.Web;
using TalentGoManagerWebApp.Models;

namespace TalentGoManagerWebApp.Controllers
{
    [Authorize(Roles = "QJYC\\招聘管理员,QJYC\\招聘监督人")]
	public class UsersController : Controller
	{
        ApplicationUserManager targetUserManager;

        public UsersController(ApplicationUserManager targetUserManager)
        {
            this.targetUserManager = targetUserManager;
        }

		// GET: Mgmt/Users
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult UserList(UserListViewModel model)
		{
			if (model == null)
			{
				model = new UserListViewModel()
				{
					PageIndex = 0
				};
			}
			int allCount;
			model.AppUserList = this.GetSelectedUsers(model.Keywords, model.OrderColumn, model.DownDirection, model.PageIndex, model.PageSize, out allCount);

			model.AllCount = allCount;

			return View(model);
		}


		public IQueryable<Person> GetSelectedUsers(string Keywords, string OrderColumn, bool DownDirection, int PageIndex, int PageSize, out int ItemCount)
		{
            ///带分页
            ///
            //先获得符合初始条件的集合
            var userSet = this.targetUserManager.Users;

			//if (userSet.Count() <= 0)
			//{
			//    ItemCount = 0;
			//    return userSet;
			//}

			//模糊查询
			if (!string.IsNullOrEmpty(Keywords))
				userSet = userSet.Where(e =>
					e.DisplayName.StartsWith(Keywords) ||
					e.IDCardNumber.StartsWith(Keywords) ||
					e.Mobile.StartsWith(Keywords) ||
					e.Email.StartsWith(Keywords)
				);


			ItemCount = userSet.Count();
			if (ItemCount == 0)
			{
				return userSet;
			}
			//按字段排序
			if (string.IsNullOrEmpty(OrderColumn))
				OrderColumn = "WhenCreated";


			if (DownDirection)
				userSet = userSet.OrderByDescending(OrderColumn);
			else
				userSet = userSet.OrderBy(OrderColumn);


			//检查PageIndex和PageSize是否符合要求
			if (PageSize <= -1)
				PageSize = int.MaxValue;
			if (PageSize >= 0 && PageSize < 5)
				PageSize = 5;

			int PageCount = (int)Math.Ceiling((double)ItemCount / (double)PageSize);

			if (PageIndex < 0)
				PageIndex = 0;
			if (PageIndex >= PageCount)
				PageIndex = PageCount - 1;

			//返回指定分页的条目
			return userSet.Skip(PageIndex * PageSize).Take(PageSize);


			//ItemCount = 0;
			//return new List<Enrollment>().AsEnumerable();

		}
	}
}