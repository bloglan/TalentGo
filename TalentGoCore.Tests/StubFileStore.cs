using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentGo;

namespace TalentGoCore.Tests
{
    class StubFileStore : IFileStore
    {
        List<File> innerList;

        public StubFileStore()
        {
            this.innerList = new List<File>();
        }

        public IQueryable<File> Files => this.innerList.AsQueryable();

        public Task CreateAsync(File file)
        {
            this.innerList.Add(file);
            return Task.FromResult(0);
        }

        public Task DeleteAsync(File file)
        {
            this.innerList.Remove(file);
            return Task.FromResult(0);
        }

        public Task<bool> ExistsAsync(string id)
        {
            return Task.FromResult(this.innerList.Any(f => f.Id == id));
        }

        public Task<File> FindByIdAsync(string id)
        {
            return Task.FromResult(this.innerList.FirstOrDefault(f => f.Id == id));
        }

        public Task UpdateAsync(File file)
        {
            //DoNothing
            return Task.FromResult(0);
        }
    }
}
