using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentGo;

namespace TalentGoCore.Tests
{
    class StubApplicationFormStore : IApplicationFormStore
    {
        List<ApplicationForm> innerList;

        public StubApplicationFormStore()
        {
            this.innerList = new List<ApplicationForm>();
        }

        public IQueryable<ApplicationForm> ApplicationForms => this.innerList.AsQueryable();

        public Task CreateAsync(ApplicationForm form)
        {
            var lastForm = this.innerList.OrderByDescending(i => i.Id).FirstOrDefault();
            var lastId = lastForm != null ? lastForm.Id : 0;
            new PrivateObject(form).SetProperty(nameof(form.Id), lastId + 1);
            this.innerList.Add(form);
            return Task.FromResult(0);
        }

        public Task DeleteAsync(ApplicationForm form)
        {
            this.innerList.Remove(form);
            return Task.FromResult(0);
        }

        public Task<ApplicationForm> FindByIdAsync(int id)
        {
            return Task.FromResult(this.innerList.FirstOrDefault(f => f.Id == id));
        }

        public Task UpdateAsync(ApplicationForm form)
        {
            //do nothing
            return Task.FromResult(0);
        }
    }
}
