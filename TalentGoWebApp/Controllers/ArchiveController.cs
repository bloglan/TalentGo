﻿using System;
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
using TalentGo.Extension;
using TalentGo.Web;
using TalentGo;

namespace TalentGoWebApp.Controllers
{
    [Authorize]
	public class ArchiveController : Controller
	{
        ApplicationFormManager enrollmentManager;
        RecruitmentPlanManager recruitmentPlanManager;

        public ArchiveController( ApplicationFormManager enrollmentManager, RecruitmentPlanManager recruitmentPlanManager)
        {
            this.enrollmentManager = enrollmentManager;
            this.recruitmentPlanManager = recruitmentPlanManager;
        }


		/// <summary>
		/// 上传文件。
		/// </summary>
		/// <param name="planid"></param>
		/// <param name="userid"></param>
		/// <param name="acid"></param>
		/// <returns></returns>
		//[HttpPost]
		//public async Task<ActionResult> UploadFiles(int planid, Guid userid, int acid)
		//{

  //          if (Request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");

		//	var file = this.Request.Files[0];

  //          var plan = await this.recruitmentPlanManager.FindByIdAsync(planid);

  //          var currentArchReq = (await this.recruitmentPlanManager.GetArchiveRequirements(plan)).FirstOrDefault(a => a.ArchiveCategoryID == acid);
		//	if (currentArchReq == null)
		//	{
		//		return Json(new { name = "找不到文档需求。", id = 0 }, "text/plain");
		//	}
		//	var archiveCategory = currentArchReq.ArchiveCategory;

		//	RequirementType reqType = (RequirementType)Enum.Parse(typeof(RequirementType), currentArchReq.Requirements);
  //          //获取报名表对应文档

  //          var enrollment = this.enrollmentManager.ApplicationForms.FirstOrDefault(enroll => enroll.JobId == plan.Id && enroll.PersonId == this.CurrentUser().Id);
  //          var enrollmentArchiveSet = (await this.enrollmentManager.GetEnrollmentArchives(enrollment)).Where(ach => ach.ArchiveCategoryID == acid);

  //          if (enrollmentArchiveSet.Any())
		//	{
		//		if (!reqType.IsMultipleEnabled())
		//		{
		//			return Json(new { name = "文档已存在并且不允许传送多个。", id = 0 }, "text/plain");
		//		}
		//		else
		//		{
		//			if (enrollmentArchiveSet.Count() >= 10)
		//				return Json(new { name = "最多允许上传10份文档。", id = 0 }, "text/plain");
		//		}
				
		//	}

		//	List<string> Errors = new List<string>();
		//	var inputStream = file.InputStream;

		//	if (archiveCategory.MinFileSize.HasValue && file.ContentLength <= archiveCategory.MinFileSize.Value)
		//	{
		//		Errors.Add("文件大小小于" + archiveCategory.MinFileSize.Value.ToByteSize());
		//	}

		//	if (archiveCategory.MaxFileSize.HasValue && file.ContentLength > archiveCategory.MaxFileSize.Value)
		//	{
		//		Errors.Add("文件大小不能超过" + archiveCategory.MaxFileSize.Value.ToByteSize());
		//	}

		//	//初始化一个MemoryStream用于接收文件并缓存到内存。
		//	using (var ms = new MemoryStream())
		//	{

		//		inputStream.CopyTo(ms);

		//		//验证是否是图像文件
		//		ms.Position = 0;
		//		Image img = null;
		//		try
		//		{
		//			img = Image.FromStream(ms);
		//		}
		//		catch
		//		{
		//			//throw new ArgumentException("所传送的文件不是图像。");
		//			return Json(new { id = 0, name = "所传送的文件不是图像。" }, "text/plain");
		//		}

		//		if (!img.RawFormat.Equals(ImageFormat.Jpeg) && !img.RawFormat.Equals(ImageFormat.Png))
		//		{
		//			//throw new ArgumentException("不是有效的图像格式，只允许传送jpeg(jpg)图像或png图像。");

		//			return Json(new { id = 0, name = "不是有效的图像格式，只允许传送jpeg(jpg)图像或png图像。" }, "text/plain");
		//		}

				
		//		if (archiveCategory.MinHeight.HasValue && img.Size.Height < archiveCategory.MinHeight.Value)
		//			Errors.Add("图像高度应大于" + archiveCategory.MinHeight.Value + "像素");
		//		if (archiveCategory.MaxHeight.HasValue && img.Size.Height >= archiveCategory.MaxHeight.Value)
		//			Errors.Add("图像高度应不超过" + archiveCategory.MaxHeight.Value + "像素");
		//		if (archiveCategory.MinWidth.HasValue && img.Size.Width < archiveCategory.MinWidth.Value)
		//			Errors.Add("图像宽度应大于" + archiveCategory.MinWidth.Value + "像素");
		//		if (archiveCategory.MaxWidth.HasValue && img.Size.Width >= archiveCategory.MaxWidth.Value)
		//			Errors.Add("图像宽度应不超过" + archiveCategory.MaxWidth.Value + "像素");

		//		if (Errors.Count != 0)
		//		{
		//			StringBuilder sbErrors = new StringBuilder();
		//			foreach (string err in Errors)
		//			{
		//				sbErrors.Append("，" + err);
		//			}
		//			return Json(new { id = 0, name = "上传失败" + sbErrors.ToString() }, "text/plain");
		//		}

		//		//构造EnrollmentArchive对象并更新。
		//		EnrollmentArchive enrollmentArch = new EnrollmentArchive();
		//		enrollmentArch.ArchiveCategoryID = archiveCategory.Id;
		//		enrollmentArch.RecruitPlanID = plan.Id;
		//		enrollmentArch.UserID = 0;
		//		enrollmentArch.Title = string.Empty; //Keep Title as empty string. this property shold be used in the future.

		//		//从MemoryStream输出字节序列
		//		byte[] FileDataBytes = new byte[ms.Length];
		//		ms.Position = 0;
		//		ms.Read(FileDataBytes, 0, (int)ms.Length);

		//		enrollmentArch.MimeType = file.ContentType;
		//		enrollmentArch.ArchiveData = FileDataBytes;

		//		//await this.enrollmentManager.AddEnrollmentArchive(enrollment, enrollmentArch);

		//		ms.Flush();
		//		ms.Close();

		//		return Json(new { name = archiveCategory.Name, id = enrollmentArch.Id }, "text/plain");

		//	}
		//}


		#region 帮助方法

		

		#endregion

	}
}