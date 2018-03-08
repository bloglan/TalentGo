using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TalentGo;

namespace TalentGoManagerWebApp.Controllers
{
    public class FileController : Controller
    {
        IFileStore store;

        public FileController(IFileStore store)
        {
            this.store = store;
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
    }
}