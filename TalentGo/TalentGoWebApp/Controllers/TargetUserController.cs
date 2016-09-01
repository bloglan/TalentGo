using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using TalentGo.EntityFramework;
using TalentGo.Identity;
using TalentGo.Utilities;
using TalentGoWebApp.Models;

namespace TalentGoWebApp.Controllers
{
	[Authorize(Roles = "InternetUser,QJYC\\招聘登记员,QJYC\\招聘管理员")]
	public class TargetUserController : Controller
	{
		TargetUserManager targetUserManager;
		TalentGoDbContext database;

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
			this.targetUserManager = new TargetUserManager(requestContext.HttpContext);
			this.database = TalentGoDbContext.FromContext(requestContext.HttpContext);
		}
		// GET: TargetUser
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult CreateUser()
		{
			return View("CreateUser");
		}

		public async Task<ActionResult> AssignTargetUser()
		{
			return View(await this.targetUserManager.GetAvaiableTargetUsers());
		}

		/// <summary>
		/// 提交绑定用户。
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult> AssignTargetUser(int SelectedUserID)
		{
			try
			{
				await this.targetUserManager.SetTargetUserByID(SelectedUserID);
				return RedirectToAction("Index", "Recruitment");
			}
			catch (Exception ex)
			{
				return View(await this.targetUserManager.GetAvaiableTargetUsers());
			}
		}



		[HttpPost]
		public async Task<ActionResult> CreateUser(CreateTargetUserViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			
			ChineseIDCardNumber cardNumber = null;
			try
			{
				cardNumber = ChineseIDCardNumber.CreateNumber(model.IDCardNumber);
			}
			catch (Exception ex)
			{
				this.ModelState.AddModelError("IDCardNumber", "无效的身份证号码。");
				return View(model);
			}

			List<KeyValuePair<string,string>> Errors = new List<KeyValuePair<string, string>>();
			var user = this.database.Users.FirstOrDefault(e => e.UserName == cardNumber.IDCardNumber);
			if (user != null)
			{
				Errors.Add(new KeyValuePair<string, string>("IDCardNumber", "该身份证号码已被注册。"));
			}

			if (this.database.Users.SingleOrDefault(e => e.Email == model.Email) != null)
			{
				Errors.Add(new KeyValuePair<string, string>("Email", "该邮件地址已被注册。"));
			}
			if (this.database.Users.SingleOrDefault(e => e.Mobile == model.Mobile) != null)
			{
				Errors.Add(new KeyValuePair<string, string>("Mobile", "该手机号码已被注册。"));
			}

			if(Errors.Count != 0)
			{
				foreach (var err in Errors)
					this.ModelState.AddModelError(err.Key, err.Value);

				return View(model);
			}

			TargetUser newuser = new TargetUser()
			{
				IDCardNumber = model.IDCardNumber,
				Mobile = model.Mobile,
				Email = model.Email,
				DisplayName = model.DisplayName,
				
			};
			try
			{
				await this.targetUserManager.CreateTargetUser(newuser);
				return RedirectToAction("AssignTargetUser");
			}
			catch(DbEntityValidationException validationException)
			{
				foreach (var result in validationException.EntityValidationErrors)
				{
					foreach (var err in result.ValidationErrors)
					{
						this.ModelState.AddModelError("", err.ErrorMessage);
					}
				}
				return View(model);
			}
			catch(Exception ex)
			{
				this.ModelState.AddModelError("", ex.Message);
				return View(model);
			}
			
		}
	}
}