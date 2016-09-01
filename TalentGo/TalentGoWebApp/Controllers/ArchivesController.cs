using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TalentGo.Recruitment;
using TalentGo.Extension;
using TalentGo.EntityFramework;

namespace TalentGoWebApp.Controllers
{
	[Authorize(Roles = "InternetUser,QJYC\\招聘登记员,QJYC\\招聘管理员")]
	public class ArchivesController : Controller
	{
		ArchiveManager archiveManager;
		TalentGoDbContext database;

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
			this.archiveManager = new ArchiveManager(requestContext.HttpContext);
			this.database = TalentGoDbContext.FromContext(requestContext.HttpContext);
		}
		// GET: Archives
		public ActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// 上传文件。
		/// </summary>
		/// <param name="planid"></param>
		/// <param name="userid"></param>
		/// <param name="acid"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult> UploadFiles(int planid, int userid, int acid)
		{
			if (Request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");

			var file = this.Request.Files[0];

			

			var currentArchReq = this.database.ArchiveRequirements.FirstOrDefault(e => e.ArchiveCategoryID == acid && e.RecruitmentPlanID == planid);
			if (currentArchReq == null)
			{
				return Json(new { name = "找不到文档需求。", id = 0 }, "text/plain");
			}
			var archiveCategory = currentArchReq.ArchiveCategory;

			RequirementType reqType = (RequirementType)Enum.Parse(typeof(RequirementType), currentArchReq.Requirements);
			//获取报名表对应文档
			var enrollmentArchiveSet = from enrollmentArch in this.database.EnrollmentArchives
									   where enrollmentArch.RecruitPlanID == planid && enrollmentArch.UserID == userid && enrollmentArch.ArchiveCategoryID == acid
									   select enrollmentArch;
			if (enrollmentArchiveSet.Any())
			{
				if (!reqType.IsMultipleEnabled())
				{
					return Json(new { name = "文档已存在并且不允许传送多个。", id = 0 }, "text/plain");
				}
				else
				{
					if (enrollmentArchiveSet.Count() >= 10)
						return Json(new { name = "最多允许上传10份文档。", id = 0 }, "text/plain");
				}
				
			}

			List<string> Errors = new List<string>();
			var inputStream = file.InputStream;

			if (archiveCategory.MinFileSize.HasValue && file.ContentLength <= archiveCategory.MinFileSize.Value)
			{
				Errors.Add("文件大小小于" + archiveCategory.MinFileSize.Value.ToByteSize());
			}

			if (archiveCategory.MaxFileSize.HasValue && file.ContentLength > archiveCategory.MaxFileSize.Value)
			{
				Errors.Add("文件大小不能超过" + archiveCategory.MaxFileSize.Value.ToByteSize());
			}

			//初始化一个MemoryStream用于接收文件并缓存到内存。
			using (var ms = new MemoryStream())
			{

				inputStream.CopyTo(ms);

				//验证是否是图像文件
				ms.Position = 0;
				Image img = null;
				try
				{
					img = Image.FromStream(ms);
				}
				catch
				{
					//throw new ArgumentException("所传送的文件不是图像。");
					return Json(new { id = 0, name = "所传送的文件不是图像。" }, "text/plain");
				}

				if (!img.RawFormat.Equals(ImageFormat.Jpeg) && !img.RawFormat.Equals(ImageFormat.Png))
				{
					//throw new ArgumentException("不是有效的图像格式，只允许传送jpeg(jpg)图像或png图像。");

					return Json(new { id = 0, name = "不是有效的图像格式，只允许传送jpeg(jpg)图像或png图像。" }, "text/plain");
				}

				
				if (archiveCategory.MinHeight.HasValue && img.Size.Height < archiveCategory.MinHeight.Value)
					Errors.Add("图像高度应大于" + archiveCategory.MinHeight.Value + "像素");
				if (archiveCategory.MaxHeight.HasValue && img.Size.Height >= archiveCategory.MaxHeight.Value)
					Errors.Add("图像高度应不超过" + archiveCategory.MaxHeight.Value + "像素");
				if (archiveCategory.MinWidth.HasValue && img.Size.Width < archiveCategory.MinWidth.Value)
					Errors.Add("图像宽度应大于" + archiveCategory.MinWidth.Value + "像素");
				if (archiveCategory.MaxWidth.HasValue && img.Size.Width >= archiveCategory.MaxWidth.Value)
					Errors.Add("图像宽度应不超过" + archiveCategory.MaxWidth.Value + "像素");

				if (Errors.Count != 0)
				{
					StringBuilder sbErrors = new StringBuilder();
					foreach (string err in Errors)
					{
						sbErrors.Append("，" + err);
					}
					return Json(new { id = 0, name = "上传失败" + sbErrors.ToString() }, "text/plain");
				}

				//构造EnrollmentArchive对象并更新。
				EnrollmentArchives enrollmentArch = new EnrollmentArchives();
				enrollmentArch.ArchiveCategoryID = acid;
				enrollmentArch.RecruitPlanID = planid;
				enrollmentArch.UserID = userid;
				enrollmentArch.Title = string.Empty;

				//从MemoryStream输出字节序列
				byte[] FileDataBytes = new byte[ms.Length];
				ms.Position = 0;
				ms.Read(FileDataBytes, 0, (int)ms.Length);

				enrollmentArch.MimeType = file.ContentType;
				enrollmentArch.ArchiveData = FileDataBytes;

				await this.archiveManager.AddOrUpdateArchive(enrollmentArch);

				ms.Flush();
				ms.Close();

				return Json(new { name = archiveCategory.Name, id = enrollmentArch.id }, "text/plain");

			}
		}

		public async Task<ActionResult> RemoveFile(int eaid)
		{
			var enrollmentArchives = await this.archiveManager.GetEnrollmentArchives();
			var current = enrollmentArchives.SingleOrDefault(e => e.id == eaid);

			if (current == null)
			{
				return Json(new { code = 404, msg = "找不到指定文档" }, "text/plain", JsonRequestBehavior.AllowGet);
			}
			this.database.EnrollmentArchives.Remove(current);
			try
			{
				await this.database.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				return Json(new { code = 500, msg = ex.Message }, "text/plain", JsonRequestBehavior.AllowGet);
			}
			return Json(new { code = 0, msg = "操作成功" }, "text/plain", JsonRequestBehavior.AllowGet);
		}

		#region 帮助方法

		

		#endregion

	}
}