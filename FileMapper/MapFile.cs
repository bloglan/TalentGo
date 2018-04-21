using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentGo;
using TalentGo.EntityFramework;

namespace FileMapper
{
    class MapFile
    {
        TalentGoDbContext db;
        ICandidateStore candidateStore;
        IFileStore fileStore;
        string dirPath = "d:\\headimg\\";

        public MapFile()
        {
            db = new TalentGoDbContext();
            candidateStore = new CandidateStore(db);
            fileStore = new FileStore(db);
        }

        public void Load()
        {
            try
            {
                if (!System.IO.Directory.Exists(dirPath))
                    System.IO.Directory.CreateDirectory(dirPath);

                foreach (var candidate in candidateStore.Candidates)
                {
                    var file = fileStore.Files.FirstOrDefault(f => f.Id == candidate.HeadImageFile);
                    if (file != null)
                    {
                        var suffix = EnsureSuffix(file.MimeType);
                        using (var fileStream = System.IO.File.OpenWrite(dirPath + file.Id + suffix))
                        {
                            fileStream.Write(file.Data, 0, file.Data.Length);
                            fileStream.Flush();
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        string EnsureSuffix(string mime)
        {
            switch(mime.ToLower())
            {
                case "image/jpeg":
                    return ".jpg";
                case "image/png":
                    return ".png";
                default:
                    return string.Empty;
            }
        }

        public void Save()
        {
            if (!System.IO.Directory.Exists(dirPath))
                return;

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(dirPath);
            var files = dir.EnumerateFiles("*.??g");
            foreach(var file in files)
            {
                var fileNameParts = file.Name.Split('.');
                var mime = EnsureMIME(fileNameParts[1]);
                var id = fileNameParts[0];
                var dataFile = fileStore.Files.FirstOrDefault(f => f.Id == id);

                using (var fileStream = file.OpenRead())
                {
                    var dataBytes = new byte[fileStream.Length];
                    fileStream.Read(dataBytes, 0, dataBytes.Length);
                    dataFile.Data = dataBytes;
                    fileStore.UpdateAsync(dataFile).GetAwaiter().GetResult();
                }
            }

        }

        string EnsureMIME(string suffix)
        {
            switch (suffix.ToLower())
            {
                case "jpg":
                    return "image/jpeg";
                case "png":
                    return "image/png";
                default:
                    return string.Empty;
            }
        }
    }
}
