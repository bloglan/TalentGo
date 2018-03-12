using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Services
{
    /// <summary>
    /// 阿里云身份证识别服务。
    /// </summary>
    public class AliyunIDCardOCRService : IIDCardOCRService
    {
        JsonSerializer serializer;

        public AliyunIDCardOCRService()
        {
            this.serializer = new JsonSerializer();
        }
        /// <summary>
        /// 识别身份证背面（国徽面）。
        /// </summary>
        /// <param name="idCardBackImageData"></param>
        /// <returns></returns>
        public async Task<IDCardBackOCRResult> RecognizeIDCardBack(Stream idCardBackImageData)
        {
            string imgBase64;
            using (var ms = new MemoryStream())
            {
                idCardBackImageData.CopyTo(ms);
                imgBase64 = Convert.ToBase64String(ms.ToArray());
            }
            var requestData = new
            {
                image = imgBase64,
                configure = new
                {
                    side = "back",
                },
            };
            var request = WebRequest.Create(svcUrl);
            request.Method = "POST";
            request.Headers.Add("Authorization", "APPCODE " + appCode);
            request.ContentType = "application/json; charset=UTF-8";
            var requestStream = await request.GetRequestStreamAsync();
            var writer = new JsonTextWriter(new StreamWriter(requestStream));
            this.serializer.Serialize(writer, requestData);

            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();
            var result = this.serializer.Deserialize<dynamic>(new JsonTextReader(new StreamReader(responseStream)));
            if (!result.success)
                throw new IDCardRecognizeException("Can not recognize");
            var returnResult = new IDCardBackOCRResult
            {
                Issuer = result.issue,
                IssueDate = this.parseDate(result.start_date),
                ExpiresDate = this.parseDate(result.end_date),
            };
            return returnResult;
        }

        
        /// <summary>
        /// 识别身份证正面（个人信息面）。
        /// </summary>
        /// <param name="idCardFrontImageData"></param>
        /// <returns></returns>
        public async Task<IDCardFrontOCRResult> RecognizeIDCardFront(Stream idCardFrontImageData)
        {
            string imgBase64;
            using (var ms = new MemoryStream())
            {
                idCardFrontImageData.CopyTo(ms);
                imgBase64 = Convert.ToBase64String(ms.ToArray());
            }
            var requestData = new
            {
                image = imgBase64,
                configure = new
                {
                    side = "face",
                },
            };
            var request = WebRequest.Create(svcUrl);
            request.Method = "POST";
            request.Headers.Add("Authorization", "APPCODE " + appCode);
            request.ContentType = "application/json; charset=UTF-8";
            var requestStream = await request.GetRequestStreamAsync();
            var writer = new JsonTextWriter(new StreamWriter(requestStream));
            this.serializer.Serialize(writer, requestData);

            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();
            var result = this.serializer.Deserialize<dynamic>(new JsonTextReader(new StreamReader(responseStream)));
            if (!result.success)
                throw new IDCardRecognizeException("Can not recognize");
            var returnResult = new IDCardFrontOCRResult
            {
                Name = result.name,
                SexString = result.sex,
                Nationality = result.nationality,
                DateOfBirth = this.parseDate(result.birth),
                Address = result.address,
                IDCardNumber = result.num,
            };
            return returnResult;
        }

        DateTime parseDate(string dateString)
        {
            var year = int.Parse(dateString.Substring(0, 4));
            var month = int.Parse(dateString.Substring(4, 2));
            var day = int.Parse(dateString.Substring(6, 2));
            return new DateTime(year, month, day);
        }

        static string svcUrl = "https://dm-51.data.aliyun.com/rest/160601/ocr/ocr_idcard.json";
        static string appCode = "6bda48daec8c4a6b96f6a69fdfe43913";
    }
}
