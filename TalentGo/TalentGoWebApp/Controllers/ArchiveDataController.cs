using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using TalentGo.EntityFramework;
using TalentGo.Recruitment;

namespace TalentGoWebApp.Controllers
{
	[Authorize(Roles = "InternetUser,QJYC\\招聘登记员,QJYC\\招聘管理员,QJYC\\招聘监督人")]
	public class ArchiveDataController : Controller
    {
		TalentGoDbContext database;
		EnrollmentManager enrollmentManager;
		ArchiveManager archiveManager;

		protected override void Initialize(RequestContext requestContext)
		{

			base.Initialize(requestContext);
			this.enrollmentManager = new EnrollmentManager(requestContext.HttpContext);
			this.archiveManager = new ArchiveManager(requestContext.HttpContext);
			this.database = TalentGoDbContext.FromContext(requestContext.HttpContext);
		}
		// GET: ArchiveData
		public async Task<ActionResult> HeadImage(int planid, int userid)
        {
			if (this.User.Identity is WindowsIdentity && (this.User.IsInRole("QJYC\\招聘管理员") || this.User.IsInRole("QJYC\\招聘监督人")))
			{
				var arch = this.database.EnrollmentArchives.FirstOrDefault(e => e.RecruitPlanID == planid && e.UserID == userid && e.ArchiveCategoryID == 5);
				if (arch == null)
					return File("~/Content/WebRes/NoHeadImage.jpg", "image/jpeg");
				return File(arch.ArchiveData, arch.MimeType);
			}
			var enrollarchives = await this.archiveManager.GetEnrollmentArchives();
			EnrollmentArchives data = enrollarchives.SingleOrDefault(e => e.ArchiveCategoryID == 5);
			if (data == null)
				return File("~/Content/WebRes/NoHeadImage.jpg", "image/jpeg");

            return File(data.ArchiveData, data.MimeType);
        }

		public async Task<ActionResult> GetEnrollmentArchiveData(int eaid)
		{
			if (this.User.Identity is WindowsIdentity && (this.User.IsInRole("QJYC\\招聘管理员") || this.User.IsInRole("QJYC\\招聘监督人")))
			{
				var arch = this.database.EnrollmentArchives.SingleOrDefault(e => e.id == eaid);
				if (arch == null)
					return File("~/Content/WebRes/NoHeadImage.jpg", "image/jpeg");
				return File(arch.ArchiveData, arch.MimeType);
			}

			var enrollmentArchiveSet = await this.archiveManager.GetEnrollmentArchives();
			var current = enrollmentArchiveSet.SingleOrDefault(e => e.id == eaid);
			if (current == null)
				return HttpNotFound();

			return File(current.ArchiveData, current.MimeType);
		}

		public async Task<ActionResult> GetSampleImage(int acid)
		{
			var current = this.database.ArchiveCategory.Single(e => e.id == acid);
			if (current == null)
				return HttpNotFound();

			if (string.IsNullOrEmpty(current.MimeType))
				return HttpNotFound();

			return File(current.SampleImage, current.MimeType);
		}
    }
}