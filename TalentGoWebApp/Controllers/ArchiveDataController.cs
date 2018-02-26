using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using TalentGo;
using TalentGo.Web;

namespace TalentGoWebApp.Controllers
{
    [Authorize(Roles = "InternetUser,QJYC\\招聘登记员,QJYC\\招聘管理员,QJYC\\招聘监督人")]
	public class ArchiveDataController : Controller
    {
		EnrollmentManager enrollmentManager;
        ArchiveCategoryManager archiveCategoryManager;

        public ArchiveDataController(EnrollmentManager enrollmentManager, ArchiveCategoryManager archiveCategoryManager)
        {
            this.enrollmentManager = enrollmentManager;
            this.archiveCategoryManager = archiveCategoryManager;
        }

		// GET: ArchiveData
		public async Task<ActionResult> HeadImage(int planid, int userid)
        {
            var recruitmentContext = this.HttpContext.GetRecruitmentContext();
            var enrollment = this.enrollmentManager.Enrollments.FirstOrDefault(e => e.RecruitPlanID == planid && e.UserID == userid);

            if (this.User.IsInRole("QJYC\\招聘管理员") || this.User.IsInRole("QJYC\\招聘监督人"))
			{
                
                var arch = (await this.enrollmentManager.GetEnrollmentArchives(enrollment)).FirstOrDefault(a => a.ArchiveCategoryID == 5);
				if (arch == null)
					return File("~/Content/WebRes/NoHeadImage.jpg", "image/jpeg");
				return File(arch.ArchiveData, arch.MimeType);
			}

			var enrollarchives = await this.enrollmentManager.GetEnrollmentArchives(enrollment);
			EnrollmentArchive data = enrollarchives.SingleOrDefault(e => e.ArchiveCategoryID == 5);
			if (data == null)
				return File("~/Content/WebRes/NoHeadImage.jpg", "image/jpeg");

            return File(data.ArchiveData, data.MimeType);
        }

		public async Task<ActionResult> GetEnrollmentArchiveData(int eaid)
		{
			if (this.User.Identity is WindowsIdentity && (this.User.IsInRole("QJYC\\招聘管理员") || this.User.IsInRole("QJYC\\招聘监督人")))
			{
				var arch = this.enrollmentManager.FindEnrollmentArchiveByIdAsync(eaid);
				if (arch == null)
					return File("~/Content/WebRes/NoHeadImage.jpg", "image/jpeg");
				return File(arch.ArchiveData, arch.MimeType);
			}

			var enrollmentArchiveSet = this.enrollmentManager.FindEnrollmentArchiveByIdAsync(eaid);
			if (enrollmentArchiveSet == null)
				return HttpNotFound();

			return File(enrollmentArchiveSet.ArchiveData, enrollmentArchiveSet.MimeType);
		}

		public ActionResult GetSampleImage(int acid)
		{
			var current = this.archiveCategoryManager.ArchiveCategories.Single(e => e.id == acid);
			if (current == null)
				return HttpNotFound();

			if (string.IsNullOrEmpty(current.MimeType))
				return HttpNotFound();

			return File(current.SampleImage, current.MimeType);
		}
    }
}