using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    public interface IFileStoreProvider
    {
        Task StoreAsync(string key, Stream stream);

        Task LoadAsync(string key, Stream stream);
    }
}
