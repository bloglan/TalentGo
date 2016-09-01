using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProc
{
	public class DataExport : IService
	{
		TalentGo.EntityFramework.TalentGoDbContext database = new TalentGo.EntityFramework.TalentGoDbContext();
		public void Run()
		{
			var dirInfo = Directory.CreateDirectory(DataIOConfig.Path + "\\" + DataIOConfig.PlanID.ToString("00"));

			var archCataSet = from archCata in this.database.ArchiveCategory
							  where archCata.Enabled
							  select archCata;

			foreach (var archCata in archCataSet)
			{
				var cataDir = dirInfo.CreateSubdirectory(archCata.id.ToString("00"));

				var enrollmentArchSet = from enrollarch in this.database.EnrollmentArchives
										where enrollarch.RecruitPlanID == DataIOConfig.PlanID
										&& enrollarch.ArchiveCategoryID == archCata.id
										&& enrollarch.EnrollmentData.Approved.Value
										select enrollarch;
				
				foreach (var enrollmentArchive in enrollmentArchSet)
				{
					string fileName = enrollmentArchive.id.ToString("00000");
					switch(enrollmentArchive.MimeType)
					{
						case "image/jpeg":
							fileName += ".jpg";
							break;
						case "image/png":
							fileName += ".png";
							break;
						default:
							//Do nothing here.
							break;
					}

					using (Stream fs = File.OpenWrite(cataDir.FullName + "\\" + fileName))
					{
						fs.Write(enrollmentArchive.ArchiveData, 0, enrollmentArchive.ArchiveData.Length);

						fs.Flush();
						fs.Close();
					}

					Console.WriteLine("正在导出" + cataDir.FullName + "\\" + fileName);
				}
			}

		}
	}
}
