using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProc
{
	public class DataImport : IService
	{
		TalentGo.EntityFramework.TalentGoDbContext database = new TalentGo.EntityFramework.TalentGoDbContext();

		public void Run()
		{
			DirectoryInfo baseDir = new DirectoryInfo(DataIOConfig.Path);
			var planDirs = baseDir.GetDirectories();
			foreach (var planDir in planDirs)
			{
				int planID = int.Parse(planDir.Name);

				var cataDirs = planDir.GetDirectories();
				foreach (var cataDir in cataDirs)
				{
					int catalogID = int.Parse(cataDir.Name);

					var files = cataDir.EnumerateFiles();
					foreach (var file in files)
					{
						string fileName = file.Name;
						string[] fileNameParts = fileName.Split('.');
						int eaid = int.Parse(fileNameParts[0]);
						string ftype = fileNameParts.Length == 2 ? fileNameParts[1].ToLower() : null;
						string mime = null;
						switch (ftype)
						{
							case "jpg":
								mime = "image/jpeg";
								break;
							case "png":
								mime = "image/png";
								break;
							default:
								//Do Nothing here
								break;

						}

						var enrollarch = this.database.EnrollmentArchives.SingleOrDefault(e => e.id == eaid);
						if (enrollarch != null)
						{
							if (mime != null && enrollarch.MimeType != mime)
								enrollarch.MimeType = mime;

							using (Stream fs = file.OpenRead())
							{
								int filesize = (int)fs.Length;
								byte[] fileContentBytes = new byte[filesize];
								fs.Read(fileContentBytes, 0, filesize);

								enrollarch.ArchiveData = fileContentBytes;
							}
						}
						this.database.SaveChanges();

						Console.WriteLine("正在导入" + fileName);
					}
				}
			}
		}
	}
}
