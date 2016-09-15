using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using TalentGo.Recruitment;
using TalentGo.Utilities;
using TalentGo.Web;
using TalentGoWebApp.Models;

namespace TalentGoWebApp.Controllers
{
    [Authorize(Roles = "InternetUser,QJYC\\招聘登记员,QJYC\\招聘管理员")]
	public class TargetUserController : Controller
	{
		TargetUserManager manager;
        RecruitmentContextBase recruitmentContext;

        public TargetUserController(TargetUserManager targetUserManager)
        {
            this.manager = targetUserManager;
        }

        protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
            this.recruitmentContext = this.HttpContext.GetRecruitmentContext();
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
			return View(await this.manager.GetAvaiableTargetUsers(this.recruitmentContext));
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
                var selectedTargetUser = (await this.manager.GetAvaiableTargetUsers(this.recruitmentContext)).FirstOrDefault(t => t.Id == SelectedUserID);
                if (selectedTargetUser != null)
                {
                    this.recruitmentContext.TargetUserId = selectedTargetUser.Id;
                    return RedirectToAction("Index", "Recruitment");
                }
                return View(await this.manager.GetAvaiableTargetUsers(this.recruitmentContext));
            }
			catch (Exception ex)
			{
                return View(await this.manager.GetAvaiableTargetUsers(this.recruitmentContext));
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
			if(this.manager.TargetUsers.Any(e => e.UserName == cardNumber.IDCardNumber))
			{
				Errors.Add(new KeyValuePair<string, string>("IDCardNumber", "该身份证号码已被注册。"));
			}

			if (this.manager.TargetUsers.Any(e => e.Email == model.Email))
			{
				Errors.Add(new KeyValuePair<string, string>("Email", "该邮件地址已被注册。"));
			}
			if (this.manager.TargetUsers.Any(e => e.Mobile == model.Mobile))
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
				await this.manager.CreateTargetUser(newuser, this.recruitmentContext);
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