﻿using Changingsoft.Business;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TalentGo;
using TalentGo.Identity;
using TalentGo.Web;
using TalentGoWebApp.Models;

namespace TalentGoWebApp.Controllers
{
    [Authorize()]
    public class AccountController : Controller
    {
        ApplicationSignInManager _signInManager;
        ApplicationUserManager _userManager;
        PersonManager personManager;
        //MobileValidationSessionManager mvsManager;

        public AccountController(PersonManager personManager)
        {
            this.personManager = personManager;
        }


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var person = this.UserManager.Users.FirstOrDefault(u => u.IDCardNumber == model.UserId || u.Mobile == model.UserId || u.Email == model.UserId);

            if (person == null)
            {
                ModelState.AddModelError("", "登陆失败。用户名或密码有误，或由于锁定阻止了登陆。");
                return View(model);
            }

            // 这不会计入到为执行帐户锁定而统计的登录失败次数中
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(person.UserName, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    var user = await this.UserManager.FindByNameAsync(model.UserId);
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "登陆失败。用户名或密码有误，或由于锁定阻止了登陆。");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // 要求用户已通过使用用户名/密码或外部登录名登录
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 以下代码可以防范双重身份验证代码遭到暴力破解攻击。
            // 如果用户输入错误代码的次数达到指定的次数，则会将
            // 该用户帐户锁定指定的时间。
            // 可以在 IdentityConfig 中配置帐户锁定设置
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "代码无效。");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult RegisterAgreement()
        {
            return View();
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (!Properties.Settings.Default.AllowUserRegisteration)
                return View("_OutOfService");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!Properties.Settings.Default.AllowUserRegisteration)
                return View("_OutOfService");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //先测试验证码
            //再进行其他合规测试，这样可以充分利用验证码测试的复杂性，延缓自动程序利用验证错误条件进行猜测和攻击。
            using (var client = new TalentGo.ValidationCodeSvc.VerificationCodeClient())
            {
                try
                {
                    if (!await client.VerifyAsync(model.Mobile, model.ValidateCode))
                    {
                        this.ModelState.AddModelError(nameof(model.ValidateCode), "手机验证码错误或已失效。");
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    this.ModelState.AddModelError(nameof(model.ValidateCode), "验证手机号码遇到异常：" + ex.Message);
                    return View(model);
                }
            }

            List<KeyValuePair<string, string>> Errors = new List<KeyValuePair<string, string>>();


            ///为了防止利用自动程序测试条件导致隐私泄露，我们首先进行验证码测试。只有验证码合格后，才进行唯一性判别
            if (!ChineseIDCardNumber.TryParse(model.IDCardNumber, out ChineseIDCardNumber cardNumber))
            {
                Errors.Add(new KeyValuePair<string, string>("IDCardNumber", "不是一个有效的身份证号码。"));
            }


            if (await this.UserManager.FindByNameAsync(model.IDCardNumber) != null)
            {
                Errors.Add(new KeyValuePair<string, string>("IDCardNumber", "此身份证号码已被注册。"));
            }
            if (await this.UserManager.FindByEmailAsync(model.Email) != null)
            {
                Errors.Add(new KeyValuePair<string, string>("Email", "此电子邮件地址已被注册。"));
            }

            if (await this.personManager.FindByMobileAsync(model.Mobile) != null)
            {
                Errors.Add(new KeyValuePair<string, string>("Mobile", "此手机号码已被注册。"));
            }

            //唯一性判别结束后，若有错误，抛出之。
            if (Errors.Count != 0)
            {
                foreach (var item in Errors)
                {
                    this.ModelState.AddModelError(item.Key, item.Value);
                }
                Errors.Clear();
                return View(model);
            }


            var user = new WebUser(model.IDCardNumber, model.Surname, model.GivenName, model.Mobile, model.Email)
            {
                MobileValid = true,
            };

            var result = await UserManager.CreateAsync(user, model.Password);
            //
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
                // 发送包含此链接的电子邮件
                //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "确认你的帐户", "请通过单击 <a href=\"" + callbackUrl + "\">这里</a>来确认你的帐户");

                return RedirectToAction("EditRealId");
            }

            AddErrors(result);
            return View(model);
            // 如果我们进行到这一步时某个地方出错，则重新显示表单

        }

        public ActionResult EditRealId()
        {
            var person = this.CurrentUser();
            var model = new ReadIdEditViewModel()
            {
                Surname = person.Surname,
                GivenName = person.GivenName,
                Ethnicity = person.Ethnicity,
                Address = person.Address,
                Issuer = person.Issuer,
                IssueDate = person.IssueDate,
                ExpiresAt = person.ExpiresAt,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditRealId(ReadIdEditViewModel model)
        {
            if (!this.ModelState.IsValid)
                return View(model);

            //处理用户在末尾添加了“族”字。
            if (model.Ethnicity.EndsWith("族"))
                model.Ethnicity.Remove(model.Ethnicity.Length - 1);
            if (string.IsNullOrWhiteSpace(model.Ethnicity))
            {
                this.ModelState.AddModelError(nameof(model.Ethnicity), "必须填写民族名称。民族名称末尾不用添加“族”字。");
                return View(model);
            }

            model.Issuer = model.Issuer.Trim().Replace(" ", string.Empty);
            model.Address = model.Address.Trim().Replace(" ", string.Empty);

            var user = this.CurrentUser();
            try
            {
                await this.personManager.UpdateRealIdAsync(user, model.Surname, model.GivenName, model.Ethnicity, model.Address, model.Issuer, model.IssueDate.Value, model.ExpiresAt);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(model);
            }

            return RedirectToAction("IDCardFile", "Account");
        }

        public ActionResult IDCardFile()
        {
            return View(this.CurrentUser());
        }

        /// <summary>
        /// 发送验证码。
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SendMobileValidateCode(string mobile)
        {
            //验证传入的是否是有效的手机号。

            Regex reg = new Regex(@"^[1]+[3,4,5,7,8]+\d{9}$");
            if (!reg.IsMatch(mobile))
            {
                return Json(new { code = 401, msg = "无效的手机号码。" }, "text/plain");
            }

            using (var client = new TalentGo.ValidationCodeSvc.VerificationCodeClient())
            {
                try
                {
                    var result = await client.SendAsync(mobile);
                    if (result.StatusCode == 0)
                        return Json(true);
                    return Json(new { code = result.StatusCode, msg = result.Message }, "text/plain");
                }
                catch (Exception ex)
                {
                    return Json(new { code = 500, msg = ex.Message }, "text/plain");
                }
            }
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(Guid userId, string code)
        {
            if (code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult FindPasswordViaEmail()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FindPasswordViaEmail(FindPasswordViaEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // 请不要显示该用户不存在或者未经确认
                    return View("ForgotPasswordConfirmation");
                }

                // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
                // 发送包含此链接的电子邮件
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPasswordViaEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "重置密码", "请通过单击 <a href=\"" + callbackUrl + "\">此处</a>来重置你的密码");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPasswordViaEmail(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPasswordViaEmail(ResetPasswordViaEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // 请不要显示该用户不存在
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            //清除该Session所保存的任何缓存
            this.Session.Clear();

            return RedirectToAction("Index", "Home");
        }




        [AllowAnonymous]
        public ActionResult FindPasswordViaMobile()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FindPasswordViaMobile(FindPasswordViaMobileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //为了隐藏，构造一个假的token
            string token = "EbzHFOl%2BLSZ%2B3NjS1tgZyL10hmrXA78SfDgKmU%2Fxl5sAXPsfyrsEflP3k%2FBFRL%2BUXNBNtI2XuEQLJi7GiFlMEuUtp%2FCuvgyysDuN6Us3EaVf1kyKNHdyJpx8VkwKc0BwuJ0b1pjfJKITt5UExXTidehh0%2BlyK2NuAFwouA0lVwQ%55";

            var user = await this.personManager.FindByMobileAsync(model.Mobile) as WebUser;
            if (user == null)
            {
                //不要提示用户找不到用户对象，以免被自动程序测试。
                return RedirectToAction("ResetPasswordViaMobile", "Account", new { code = token });
            }

            //创建真实的token
            token = this.UserManager.GeneratePasswordResetToken(user.Id);

            //如果手机号码没有被验证，则不发送短信
            if (!user.MobileValid)
            {
                //不要显示任何提示，以免自动程序猜测
                return RedirectToAction("ResetPasswordViaMobile", "Account", new { code = token });
            }

            //发送验证码
            //await this.phoneNumberValidationService.SendValidationCodeAsync(model.Mobile);
            using (var client = new TalentGo.ValidationCodeSvc.VerificationCodeClient())
            {
                try
                {
                    var result = await client.SendAsync(model.Mobile);
                }
                catch
                { }
            }
            return RedirectToAction("ResetPasswordViaMobile", "Account", new { code = token });


        }

        [AllowAnonymous]
        public ActionResult ResetPasswordViaMobile(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPasswordViaMobile(ResetPasswordViaMobileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            WebUser user;

            try
            {
                user = await this.personManager.FindByMobileAsync(model.Mobile) as WebUser;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            if (user == null)
            {
                //不要显示找不到用户。
                return View("ResetPasswordConfirmation");
            }
            DateTime now = DateTime.Now;

            using (var client = new TalentGo.ValidationCodeSvc.VerificationCodeClient())
            {
                try
                {
                    var validationResult = await client.VerifyAsync(model.Mobile, model.ValidateCode);
                    if (!validationResult)
                        return RedirectToAction("ResetPasswordConfirmation", "Account");

                }
                catch
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");

                }
            }
            //if (!await this.phoneNumberValidationService.ValidateAsync(model.Mobile, model.ValidateCode))
            //    return View("ResetPasswordConfirmation");

            //重置密码
            var result = await this.UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadIDCardFrontSideFile()
        {
            var person = this.CurrentUser();
            try
            {
                await this.personManager.UploadIDCardFrontFileAsync(person, this.Request.Files[0].InputStream);
                return Json(new { result = 0, src = Url.Action("Index", "File", new { id = person.IDCardFrontFile }) });
            }
            catch (Exception ex)
            {
                return Json(new { result = -1, ex.Message });
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadIDCardBackSideFile()
        {
            var person = this.CurrentUser();
            try
            {
                await this.personManager.UploadIDCardBackFileAsync(person, this.Request.Files[0].InputStream);
                return Json(new { result = 0, src = Url.Action("Index", "File", new { id = person.IDCardBackFile }) });
            }
            catch (Exception ex)
            {
                return Json(new { result = -1, ex.Message });
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> RemoveIDCardFrontFile()
        {
            var person = this.CurrentUser();
            try
            {
                await this.personManager.RemoveIDCardFrontFile(person);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> RemoveIDCardBackFile()
        {
            var person = this.CurrentUser();
            try
            {
                await this.personManager.RemoveIDCardBackFile(person);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CommitForRealIdValidate()
        {
            var person = this.CurrentUser();
            try
            {
                await this.personManager.CommitForRealIdValidationAsync(person);
                return View("CommitForReadIdSuccess", model: person.RealIdValid.HasValue && person.RealIdValid.Value);
            }
            catch (Exception ex)
            {
                return View("CommitForReadIdFailed", model: ex.Message);
            }
        }

        #region 帮助程序
        // 用于在添加外部登录名时提供 XSRF 保护
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}