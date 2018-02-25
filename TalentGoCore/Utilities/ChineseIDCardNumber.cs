using System;
using System.Runtime.Serialization;

namespace TalentGo.Utilities
{
    /// <summary>
    /// 表示一个中华人民共和国的居民身份证号。
    /// </summary>
    public class ChineseIDCardNumber
	{
		private static readonly int[] Weight = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
		private static readonly char[] CheckCodeMapper = new char[] { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };

		private string idCardNumber;
		private DateTime dateOfBirth;
		private string regionCode;
		private string sequenceNumber;
		private Gender gender;

		private int version;
		private CardNumberCheckMode checkMode;

		/// <summary>
		/// 从身份证号码字符串创建身份证号码。该过程将验证输入的身份证字符串合规性。
		/// </summary>
		/// <param name="IDCardNumber">输入的身份证号码字符串。</param>
		/// <returns>返回代表中国居民身份证号码的实例。</returns>
		/// <exception cref="ArgumentException">身份证验证无法通过。</exception>
		public static ChineseIDCardNumber CreateNumber(string IDCardNumber)
		{
			if (string.IsNullOrEmpty(IDCardNumber))
				throw new ArgumentException("输入是空值或空引用");

			string PreProcessNumber = IDCardNumber.Trim().ToUpper();

			switch (PreProcessNumber.Length)
			{
				case 15:
					//15位号码处理过程
					return InternalProcessNumberV1(PreProcessNumber);
				case 18:
					//18位号码处理过程
					return InternalProcessNumberV2(PreProcessNumber);
				default:
					throw new ArgumentException("输入错误。字符串参数长度必须为15或18个字符。");
			}
		}

		public static bool TryParseIDCardNumber(string IDCardNumberString, out ChineseIDCardNumber chineseIDCardNumber)
		{
			try
			{
				chineseIDCardNumber = ChineseIDCardNumber.CreateNumber(IDCardNumberString);
				return true;
			}
			catch
			{
				chineseIDCardNumber = null;
				return false;
			}
		}

		private static ChineseIDCardNumber InternalProcessNumberV1(string IDCardNumber)
		{
			foreach (char NumberChar in IDCardNumber.ToCharArray())
			{
				if (!char.IsDigit(NumberChar))
					throw new ArgumentException("输入错误。输出的值包含非法字符。");
			}

			ChineseIDCardNumber cidn = new ChineseIDCardNumber();
			cidn.version = 1;
			cidn.checkMode = CardNumberCheckMode.None;
			cidn.idCardNumber = IDCardNumber;

			cidn.regionCode = IDCardNumber.Substring(0, 6);
			string dateOfBirthString = "19" + IDCardNumber.Substring(6, 2) + "-" + IDCardNumber.Substring(8, 2) + "-" + IDCardNumber.Substring(10, 2);
			try
			{
				cidn.dateOfBirth = DateTime.Parse(dateOfBirthString);
			}
			catch (Exception ex)
			{
				throw new ArgumentException("参数错误。无法将身份证号码中的出生日期特征码分析为有效的日期。", ex);
			}

			cidn.sequenceNumber = IDCardNumber.Substring(12, 3);

			int genderIntCode = int.Parse(IDCardNumber[14].ToString());
			switch (genderIntCode)
			{
				case 0:
				case 2:
				case 4:
				case 6:
				case 8:
					cidn.gender = Gender.Female;
					break;
				case 1:
				case 3:
				case 5:
				case 7:
				case 9:
					cidn.gender = Gender.Male;
					break;
			}

			return cidn;
		}

		private static ChineseIDCardNumber InternalProcessNumberV2(string IDCardNumber)
		{
			char[] IDCardNumberChars = IDCardNumber.ToCharArray();
			for (int i = 0; i < 17; i++)
			{
				if (!char.IsDigit(IDCardNumberChars[i]))
					throw new ArgumentException("参数错误。身份证号码包含非法字符，除第18位可能为X，其余应为数字。");
			}

			if (!char.IsDigit(IDCardNumberChars[17]))
			{
				//验证是否为"X"
				if (IDCardNumberChars[17] != 'X')
					throw new ArgumentException("参数错误。身份证号码包含非法字符，除第18位可能为X，其余应为数字。");
			}

			ChineseIDCardNumber cidn = new ChineseIDCardNumber();
			cidn.version = 2;
			cidn.checkMode = CardNumberCheckMode.Legal;
			cidn.idCardNumber = IDCardNumber;

			cidn.regionCode = IDCardNumber.Substring(0, 6);
			string dateOfBirthString = IDCardNumber.Substring(6, 4) + "-" + IDCardNumber.Substring(10, 2) + "-" + IDCardNumber.Substring(12, 2);
			try
			{
				cidn.dateOfBirth = DateTime.Parse(dateOfBirthString);
			}
			catch (Exception ex)
			{
				throw new ArgumentException("参数错误。无法将身份证号码中的出生日期特征码分析为有效日期。", ex);
			}

			cidn.sequenceNumber = IDCardNumber.Substring(14, 3);

			int genderIntCode = int.Parse(IDCardNumber[16].ToString());
			switch (genderIntCode)
			{
				case 0:
				case 2:
				case 4:
				case 6:
				case 8:
					cidn.gender = Gender.Female;
					break;
				case 1:
				case 3:
				case 5:
				case 7:
				case 9:
					cidn.gender = Gender.Male;
					break;
			}

			//演算校验值。
			int CheckSum = 0;
			for (int csi = 0; csi < 17; csi++)
			{
				CheckSum += (int.Parse(IDCardNumber[csi].ToString()) * Weight[csi]);
			}
			CheckSum = CheckSum % 11;
			char cacCheckCode = CheckCodeMapper[CheckSum];

			if (cacCheckCode != IDCardNumber[17])
				throw new ArgumentException("参数错误。校验错误。");

			return cidn;
		}

		private ChineseIDCardNumber()
		{
		}

		/// <summary>
		/// 获取身份证号码。
		/// </summary>
		public string IDCardNumber
		{
			get { return this.idCardNumber; }
		}

		/// <summary>
		/// 获取身份证上的出生年月日。
		/// </summary>
		public DateTime DateOfBirth
		{
			get { return this.dateOfBirth; }
		}

		/// <summary>
		/// 获取区域代码。
		/// </summary>
		public string RegionCode
		{
			get { return this.regionCode; }
		}

		/// <summary>
		/// 获取身份证序列号。
		/// </summary>
		public string SequenceNumber
		{
			get { return this.sequenceNumber; }
		}

		/// <summary>
		/// 获取性别。
		/// </summary>
		public Gender Gender
		{
			get { return this.gender; }
		}

		/// <summary>
		/// 获取身份证版本。
		/// 1代表第一代15位身份证。
		/// 2代表第二代18位身份证。
		/// </summary>
		public int Version
		{
			get { return this.version; }
		}

		/// <summary>
		/// 获取此实例代表的身份证的检查模式。
		/// </summary>
		public CardNumberCheckMode CheckMode
		{
			get { return this.checkMode; }
		}
	}

	/// <summary>
	/// 表示人的生理性别。
	/// </summary>
	[DataContract(Namespace = "http://schemas.tobaccolive.cn/ws/2011/01/common")]
	public enum Gender
	{
		/// <summary>
		/// 男性。
		/// </summary>
		[EnumMember]
		Male,
		/// <summary>
		/// 女性。
		/// </summary>
		[EnumMember]
		Female
	}

	/// <summary>
	/// 身份证号码的检查模式。
	/// </summary>
	public enum CardNumberCheckMode
	{
		/// <summary>
		/// 无检查。
		/// </summary>
		None,
		/// <summary>
		/// 法定检查。
		/// </summary>
		Legal
	}
}
