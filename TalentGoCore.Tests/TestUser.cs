using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentGo;

namespace TalentGoCore.Tests
{
    class TestUser : Person
    {
        public TestUser(string idCardNumber, string surname, string givenName, string mobile,string email)
            : base()
        {
            var cardNumber = ChineseIDCardNumber.Parse(idCardNumber);

            this.IDCardNumber = cardNumber.ToString();
            this.Sex = cardNumber.IsMale ? Sex.Male : Sex.Female;
            this.DateOfBirth = cardNumber.DateOfBirth;
            this.Surname = surname;
            this.GivenName = givenName;
            this.DisplayName = surname + givenName;
            this.Mobile = mobile;
            this.Email = email;
        }

    }
}
