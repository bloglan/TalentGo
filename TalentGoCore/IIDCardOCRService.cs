using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 身份证OCR识别服务。
    /// </summary>
    public interface IIDCardOCRService
    {
        /// <summary>
        /// 根据指定图像，识别身份证正面信息。
        /// </summary>
        /// <param name="idCardFrontImageData"></param>
        /// <returns></returns>
        Task<IDCardFrontOCRResult> RecognizeIDCardFront(Stream idCardFrontImageData);

        /// <summary>
        /// 根据指定图像，识别身份证背面信息。
        /// </summary>
        /// <param name="idCardBackImageData"></param>
        /// <returns></returns>
        Task<IDCardBackOCRResult> RecognizeIDCardBack(Stream idCardBackImageData);
    }
}
