using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TalentGo.EntityFramework;
using TalentGo.Recruitment;
using TalentGo.Utilities;

namespace TalentGoWebApp.Areas.Mgmt.Controllers
{
	[Authorize(Roles = "QJYC\\招聘管理员,QJYC\\招聘监督人")]
	public class BaseDataController : Controller
	{
		BaseDataManger baseDataManager;
		TalentGoDbContext database;

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
			this.database = TalentGoDbContext.FromContext(requestContext.HttpContext);
			this.baseDataManager = new BaseDataManger(this.database);
		}


		// GET: Mgmt/BaseData
		public ActionResult Index()
		{
			return View(this.baseDataManager.GetArchiveCategories());
		}

		public ActionResult Update(int id)
		{
			var Ac = this.baseDataManager.GetArchiveCategories().SingleOrDefault(e => e.id == id);
			if (Ac != null)
			{
				return View(Ac);
			}
			else {
				return RedirectToAction("Index");
			}

		}
		[HttpPost]
		public ActionResult Update(int id, ArchiveCategory model)
		{
			this.baseDataManager.Update(model);
			return RedirectToAction("Index");
		}

		public ActionResult Create()
		{
			return View();
		}

		// POST: Mgmt/Article/Create
		[HttpPost]
		public ActionResult Create(ArchiveCategory model)
		{
			if (Request.Files.Count != 0)
			{

				var file = this.Request.Files[0];


				var inputStream = file.InputStream;
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
						this.ModelState.AddModelError("picErr", "上传文件不是图片。");
						return View(model);
					}

					if (!img.RawFormat.Equals(ImageFormat.Jpeg) && !img.RawFormat.Equals(ImageFormat.Png))
					{
						//throw new ArgumentException("不是有效的图像格式，只允许传送jpeg(jpg)图像或png图像。");
						this.ModelState.AddModelError("picErr", "不是有效的图像格式，只允许传送jpeg(jpg)图像或png图像。");
						return View(model);
					}


					//从MemoryStream输出字节序列
					byte[] FileDataBytes = new byte[ms.Length];
					ms.Position = 0;
					ms.Read(FileDataBytes, 0, (int)ms.Length);

					//enrollmentArch.MimeType = file.ContentType;
					model.SampleImage = FileDataBytes;
					model.MimeType = file.ContentType;
					ms.Flush();
					ms.Close();
				}
			}
			model.WhenCreated = DateTime.Now;
			model.WhenChanged = DateTime.Now;

			this.database.ArchiveCategory.Add(model);
			this.database.SaveChanges();


			return RedirectToAction("Index");

		}

		public async Task<ActionResult> GetPicture(int eaid)
		{
			var item = this.database.ArchiveCategory.SingleOrDefault(e => e.id == eaid);
			if (item == null)
				return HttpNotFound();

			return File(item.SampleImage, item.MimeType);

			
		}


		[HttpPost]
		public async Task<ActionResult> UploadFiles(int acid)
		{
			if (Request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");

			var file = this.Request.Files[0];

			var archiveCategory = this.database.ArchiveCategory.SingleOrDefault(e => e.id == acid);
			if (archiveCategory == null)
				return Json(new { name = "找不到文档编目。", id = 0 }, "text/plain");


			var inputStream = file.InputStream;
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

				//从MemoryStream输出字节序列
				byte[] FileDataBytes = new byte[ms.Length];
				ms.Position = 0;
				ms.Read(FileDataBytes, 0, (int)ms.Length);

				//enrollmentArch.MimeType = file.ContentType;
				archiveCategory.SampleImage = FileDataBytes;
				archiveCategory.MimeType = file.ContentType;
				archiveCategory.WhenChanged = DateTime.Now;

				this.database.SaveChanges();

				ms.Flush();
				ms.Close();

				return Json(new { name = archiveCategory.Name, id = archiveCategory.id }, "text/plain");

			}
		}

		public async Task<ActionResult> RemoveFile(int eaid)
		{
			var current = this.database.ArchiveCategory.SingleOrDefault(e => e.id == eaid);

			if (current == null)
			{
				return Json(new { code = 404, msg = "找不到指定文档" }, "text/plain", JsonRequestBehavior.AllowGet);
			}
			current.SampleImage = null;
			current.MimeType = null;
			current.WhenChanged = DateTime.Now;
			this.baseDataManager.Update(current);
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
	}
}