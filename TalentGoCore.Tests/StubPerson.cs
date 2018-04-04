using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentGo;

namespace TalentGoCore.Tests
{
    class StubPerson : Person
    {
        public StubPerson()
            : base()
        {
            this.Id = Guid.NewGuid();
            this.IDCardNumber = "IDCardNumber";
            this.Sex = Sex.Male;
            this.DateOfBirth = new DateTime(2000, 1, 1);
            this.Surname = "Surname";
            this.GivenName = "GivenName";
            this.DisplayName = "SurnameGivenName";
            this.Mobile = "Mobile";
            this.Email = "Email";
        }


    }
}
