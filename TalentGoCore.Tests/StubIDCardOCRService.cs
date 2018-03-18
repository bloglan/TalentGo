using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentGo;

namespace TalentGoCore.Tests
{
    class StubIDCardOCRService : IIDCardOCRService
    {
        public Task<IDCardBackOCRResult> RecognizeIDCardBack(Stream idCardBackImageData)
        {
            var result = new IDCardBackOCRResult
            {
                Issuer = "Issuer",
                IssueDate = new DateTime(2008, 2, 21),
                ExpiresDate = new DateTime(2018, 2, 21),
            };
            return Task.FromResult(result);
        }

        public Task<IDCardFrontOCRResult> RecognizeIDCardFront(Stream idCardFrontImageData)
        {
            var result = new IDCardFrontOCRResult
            {
                Name = "Name",
                SexString = "男",
                Nationality = "Nationality",
                DateOfBirth = new DateTime(1985,1,15),
                Address = "Address",
                IDCardNumber = "530302198501150314",
            };
            return Task.FromResult(result);
        }
    }
}
