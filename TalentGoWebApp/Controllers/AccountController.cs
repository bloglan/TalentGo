using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TalentGoWebApp.Models;
using TalentGo.Identity;
using TalentGo.EntityFramework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net.Mail;
using TalentGo.Web;
using TalentGo;

namespace TalentGoWebApp.Controllers
{
    [Authorize()]
    public class AccountController : Controller
    {
        ApplicationSignInManager _signInManager;
        ApplicationUserManager _userManager;
        //MobileValidationSessionManager mvsManager;
        IPhoneNumberValidationService phoneNumberValidationService;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IPhoneNumberValidationService phoneNumberValidationService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            this.phoneNumberValidationService = phoneNumberValidationService;
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

            // 这不会计入到为执行帐户锁定而统计的登录失败次数中
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.IDCardNumber, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    var user = await this.UserManager.FindByNameAsync(model.IDCardNumber);
                    var context = this.HttpContext.GetRecruitmentContext();
                    context.TargetUserId = user.Id;

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

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //先测试验证码
            //再进行其他合规测试，这样可以充分利用验证码测试的复杂性，延缓自动程序利用验证错误条件进行猜测和攻击。
            if (!await this.phoneNumberValidationService.ValidateAsync(model.Mobile, model.ValidateCode))
            {
                this.ModelState.AddModelError("ValidateCode", "验证码错误或失效。");
                return View(model);
            }

            List<KeyValuePair<string, string>> Errors = new List<KeyValuePair<string, string>>();


            ///为了防止利用自动程序测试条件导致隐私泄露，我们首先进行验证码测试。只有验证码合格后，才进行唯一性判别
            ChineseIDCardNumber cardnumber;
            if (!ChineseIDCardNumber.TryParse(model.IDCardNumber, out cardnumber))
            {
                Errors.Add(new KeyValuePair<string, string>("IDCardNumber", "不是一个有效的身份证号码。"));
            }
            

            Person currentuser = null;
            currentuser = await this.UserManager.FindByNameAsync(model.IDCardNumber);
            if (currentuser != null)
            {
                Errors.Add(new KeyValuePair<string, string>("IDCardNumber", "此身份证号码已被注册。"));
            }
            currentuser = await this.UserManager.FindByEmailAsync(model.Email);
            if (currentuser != null)
            {
                Errors.Add(new KeyValuePair<string, string>("Email", "此电子邮件地址已被注册。"));
            }
            currentuser = await this.UserManager.FindByMobileAsync(model.Mobile);
            if (currentuser != null)
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


            var user = new WebUser { UserName = cardnumber.ToString(), Email = model.Email, Mobile = model.Mobile, MobileValid = true, EmailValid = false, IDCardNumber = cardnumber.ToString(), DisplayName = model.RealName };
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

                return RedirectToAction("Index", "Home");
            }

            AddErrors(result);
            return View(model);
            // 如果我们进行到这一步时某个地方出错，则重新显示表单

        }

        /// <summary>
        /// 发送验证码。
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> SendMobileValidateCode(string mobile)
        {
            //验证传入的是否是有效的手机号。

            Regex reg = new Regex(@"^[1]+[3,4,5,7,8]+\d{9}$");
            if (!reg.IsMatch(mobile))
            {
                return Json(new { code = 401, msg = "无效的手机号码。" }, "text/plain", JsonRequestBehavior.AllowGet);
            }

            var current = await this.UserManager.FindByMobileAsync(mobile);
            if (current != null)
            {
                return Json(new { code = 450, msg = "该手机号码已被使用。" }, "text/plain", JsonRequestBehavior.AllowGet);
            }

            try
            {
                await this.phoneNumberValidationService.SendValidationCodeAsync(mobile);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, "text/plain", JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 0, msg = "" }, "text/plain", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId == 0 || code == null)
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
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // 请求重定向到外部登录提供程序
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == 0)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // 生成令牌并发送该令牌
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // 如果用户已具有登录名，则使用此外部登录提供程序将该用户登录
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // 如果用户没有帐户，则提示该用户创建帐户
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // 从外部登录提供程序获取有关用户的信息
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new WebUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
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

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult SendVCodeByEmail(string Email, string Mobile)
        {
            //验证电话号码和邮箱的格式有效性。
            Regex regMobile = new Regex(@"^[1]+[3,4,5,7,8]+\d{9}$");
            if (!regMobile.IsMatch(Mobile))
            {
                return Json(new { code = 401, msg = "无效的手机号码。" }, "text/plain", JsonRequestBehavior.AllowGet);
            }

            //这里使用的是JQuery Validate中的电子邮件验证的正则表达式。
            Regex regMail = new Regex(@"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
            if (!regMail.IsMatch(Email))
            {
                return Json(new { code = 401, msg = "无效的电子邮件地址。" }, "text/plain", JsonRequestBehavior.AllowGet);
            }

            DateTime now = DateTime.Now;

            return Json(new { code = 999, msg = "不再支持。" }, "text/plain", JsonRequestBehavior.AllowGet);


            //返回成功。
        }

        void SendValidationCodeByEmail(string To, string Code)
        {
            using (var smtpClient = new SmtpClient("mail.qjyc.cn"))
            {
                smtpClient.UseDefaultCredentials = true;

                MailMessage mail = new MailMessage(new MailAddress("job@qjyc.cn", "曲靖烟草招聘"), new MailAddress(To));
                //mail.IsBodyHtml = true;
                mail.Subject = "注册验证码";
                mail.Body = "您好，欢迎访问曲靖烟草招聘网站。\r\n"
                    + "我们已留意到您无法通过手机收取验证码短信，因此，我们通过此邮件向您发送验证码。\r\n"
                    + "您的验证码是：" + Code + "。\r\n"
                    + "请在注册用户时，填入您曾用于获取此验证码的手机号码和收取此验证码的电子邮件地址。若填入其他的手机号码或电子邮件地址，验证将会失败。\r\n"
                    + "请在收到此验证码6个小时以内使用它，过期失效。\r\n"
                    + "若您在访问网站中遇到问题，请回复此邮件与我们联系。、\r\n"
                    + "若对招聘公告有疑问，请按招聘公告公布的联系方式与主管单位取得联系。谢谢，祝您使用愉快。";

                smtpClient.Send(mail);
            }
        }

        void ReportCannotReceiveVCode(string Mobile, string Email, string VCode)
        {
            using (var sms = new SMSSvc.SMSServiceClient())
            {
                string SupportMobile = "13887441196"; //李晋平 13887441196

                sms.SendMessage(new string[] { SupportMobile }, "手机号为" + Mobile + "的注册用户无法收到验证码，其当前有效验证码为" + VCode + "，请在收到此短信6小时之内致电该手机号码，协助其完成注册。谢谢。", new SMSSvc.SendMessageOption());
            }
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

            var user = await this.UserManager.FindByMobileAsync(model.Mobile);
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

            //创建验证码并发送
            await this.phoneNumberValidationService.SendValidationCodeAsync(model.Mobile);

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

            var user = await this.UserManager.FindByMobileAsync(model.Mobile);
            if (user == null)
            {
                //不要显示找不到用户。
                return View("ResetPasswordConfirmation");
            }
            DateTime now = DateTime.Now;
            if(!await this.phoneNumberValidationService.ValidateAsync(model.Mobile, model.ValidateCode))
                return View("ResetPasswordConfirmation");

            //重置密码
            var result = await this.UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

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