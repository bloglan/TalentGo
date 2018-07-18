using Changingsoft.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TalentGo;

namespace TalentGoWebApp.Controllers
{
    public class FileController : Controller
    {
        IFileStore store;
        ThumbnailPreset thumbnailPreset;

        public FileController(IFileStore store)
        {
            this.store = store;
            thumbnailPreset = new JpegThumbnailPreset(80)
            {
                ThumbnailSize = new System.Drawing.Size(200, 200),
                ThumbnailResizeMode = ResizeMode.FitCut,
                BackgroundColor = System.Drawing.Color.White,
            };
        }

        // GET: File
        public async Task<ActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var file = await this.store.FindByIdAsync(id);
            if (file == null)
                return HttpNotFound();

            return File(file.Data, file.MimeType);
        }

        public async Task<ActionResult> Thumbnail(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var file = await this.store.FindByIdAsync(id);
            if (file == null)
                return HttpNotFound();
            var outStream = new MemoryStream();
            using (var stream = new MemoryStream())
            {
                await file.WriteAsync(stream);
                stream.Position = 0;

                ThumbnailProcessor.RenderThumbnail(stream, this.thumbnailPreset, outStream);
                outStream.Position = 0;
                return File(outStream, "image/jpeg");
            }
        }
    }
}