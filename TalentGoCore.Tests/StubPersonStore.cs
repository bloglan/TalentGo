using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentGo;

namespace TalentGoCore.Tests
{
    class StubPersonStore : IPersonStore
    {
        List<Person> innerList;

        public StubPersonStore()
        {
            this.innerList = new List<Person>();
        }

        public IQueryable<Person> People => this.innerList.AsQueryable();

        public Task CreateAsync(Person person)
        {
            this.innerList.Add(person);
            return Task.FromResult(0);
        }

        public Task DeleteAsync(Person person)
        {
            this.innerList.Remove(person);
            return Task.FromResult(0);
        }

        public Task<Person> FindByIdAsync(Guid Id)
        {
            return Task.FromResult(this.innerList.FirstOrDefault(p => p.Id == Id));
        }

        public Task UpdateAsync(Person person)
        {
            //DoNothing.
            return Task.FromResult(0);
        }
    }
}
