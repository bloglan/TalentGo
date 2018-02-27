using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 
    /// </summary>
    public struct ChineseIDCardNumber
    {
        int regionCode;
        DateTime dateOfBirth;
        int serialNumber;
        bool isMale;
        char checkCode;
        int verison;

        /// <summary>
        /// 使用区划代码，出生日期和序列号初始化身份证号码。
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="serialNumber"></param>
        public ChineseIDCardNumber(int regionCode, DateTime dateOfBirth, int serialNumber)
        {
            this.checkCode = CalculateCheckCode(regionCode, dateOfBirth, serialNumber);
            this.regionCode = regionCode;
            this.dateOfBirth = dateOfBirth;
            this.serialNumber = serialNumber;
            this.isMale = this.serialNumber % 2 == 1;
            this.verison = 2;
        }

        /// <summary>
        /// 
        /// </summary>
        public int RegionCode
        {
            get { return this.regionCode; }
            set
            {
                this.regionCode = value;
                this.checkCode = CalculateCheckCode(this.regionCode, this.dateOfBirth, this.serialNumber);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime DateOfBirth
        {
            get { return this.dateOfBirth; }
            set
            {
                this.dateOfBirth = value;
                this.checkCode = CalculateCheckCode(this.regionCode, this.dateOfBirth, this.serialNumber);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SerialNumber
        {
            get { return this.serialNumber; }
            set
            {
                this.serialNumber = value;
                this.checkCode = CalculateCheckCode(this.regionCode, this.dateOfBirth, this.serialNumber);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMale
        {
            get { return this.isMale; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Version
        {
            get { return this.verison; }
        }

        /// <summary>
        /// 
        /// </summary>
        public char CheckCode
        {
            get { return this.checkCode; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.verison == 1)
                return this.regionCode.ToString("000000") + this.dateOfBirth.ToString("yyMMdd") + this.serialNumber.ToString("000");
            return this.regionCode.ToString("000000") + this.dateOfBirth.ToString("yyyyMMdd") + this.serialNumber.ToString("000") + this.checkCode.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var newHash = (long)this.regionCode.GetHashCode() + (long)this.dateOfBirth.GetHashCode() + (long)this.serialNumber.GetHashCode() + (long)this.verison.GetHashCode();
            return (int)(newHash % int.MaxValue);
        }

        /// <summary>
        /// 已重写。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ChineseIDCardNumber))
                return false;
            var target = (ChineseIDCardNumber)obj;
            return this.regionCode == target.regionCode && this.dateOfBirth == target.dateOfBirth && this.serialNumber == target.serialNumber && this.verison == target.verison;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(ChineseIDCardNumber a, ChineseIDCardNumber b)
        {
            return a.regionCode == b.regionCode && a.dateOfBirth == b.dateOfBirth && a.serialNumber == b.serialNumber && a.verison == b.verison;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(ChineseIDCardNumber a, ChineseIDCardNumber b)
        {
            return !(a.regionCode == b.regionCode && a.dateOfBirth == b.dateOfBirth && a.serialNumber == b.serialNumber && a.verison == b.verison);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool TryParse(string s, out ChineseIDCardNumber instance)
        {
            instance = new ChineseIDCardNumber(0, DateTime.MinValue, 0);
            if (string.IsNullOrWhiteSpace(s))
                return false;
            s = s.Trim();
            if (s.Length == 15)
            {
                var match = Regex.Match(s, @"^(\d{6})(\d{2})(\d{2})(\d{2})(\d{3})$", RegexOptions.IgnoreCase);
                if (!match.Success)
                    return false;

                var regionCode = int.Parse(match.Groups[1].Value);
                var year = int.Parse(match.Groups[2].Value);

                var dateOfBirth = new DateTime(1900 + year, int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
                var serialNumber = int.Parse(match.Groups[5].Value);
                instance = new ChineseIDCardNumber(serialNumber, dateOfBirth, serialNumber);
                instance.verison = 1;
                return true;
            }
            else if (s.Length == 18)
            {
                var match = Regex.Match(s, @"^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})(\d|X)$", RegexOptions.IgnoreCase);
                if (!match.Success)
                    return false;

                var regionCode = int.Parse(match.Groups[1].Value);
                var dateOfBirth = new DateTime(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
                var serialNumber = int.Parse(match.Groups[5].Value);
                var checkCode = char.Parse(match.Groups[6].Value);
                instance = new ChineseIDCardNumber(regionCode, dateOfBirth, serialNumber);
                return instance.checkCode == checkCode;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ChineseIDCardNumber Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                throw new ArgumentException("Input value is null or white spaces.");

            s = s.Trim();
            if (s.Length == 15)
            {
                var match = Regex.Match(s, @"^(\d{6})(\d{2})(\d{2})(\d{2})(\d{3})$", RegexOptions.IgnoreCase);
                if (!match.Success)
                    throw new ArgumentException("Invalid format.");

                var regionCode = int.Parse(match.Groups[1].Value);
                var year = int.Parse(match.Groups[2].Value);

                var dateOfBirth = new DateTime(1900 + year, int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
                var serialNumber = int.Parse(match.Groups[5].Value);
                var instance = new ChineseIDCardNumber(serialNumber, dateOfBirth, serialNumber);
                instance.verison = 1;
            }
            else if (s.Length == 18)
            {
                var match = Regex.Match(s, @"^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})(\d|X)$", RegexOptions.IgnoreCase);
                if (!match.Success)
                    throw new ArgumentException("Invalid format.");

                var regionCode = int.Parse(match.Groups[1].Value);
                var dateOfBirth = new DateTime(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
                var serialNumber = int.Parse(match.Groups[5].Value);
                var checkCode = char.Parse(match.Groups[6].Value);
                var instance = new ChineseIDCardNumber(regionCode, dateOfBirth, serialNumber);
                if (instance.checkCode != checkCode)
                    throw new InvalidNumberException();
                return instance;
            }
            throw new InvalidNumberException();

        }

        static char CalculateCheckCode(string number)
        {

            var sum = 0;
            for (int i = 0; i < number.Length; i++)
            {
                sum += (int.Parse(number[i].ToString()) * Weight[i]);
            }
            return CheckCodeMapper[sum % 11];
        }

        static char CalculateCheckCode(int regionCode, DateTime dateOfBirth, int serialNumber)
        {
            if (regionCode < 100000 || regionCode > 999999)
                throw new ArgumentOutOfRangeException("Region code out of range.");
            if (serialNumber < 0 || serialNumber > 999)
                throw new ArgumentOutOfRangeException("Serial number out of range.");
            var numberBuilder = new StringBuilder();
            numberBuilder.Append(regionCode.ToString("000000"));
            numberBuilder.Append(dateOfBirth.ToString("yyyyMMdd"));
            numberBuilder.Append(serialNumber.ToString("000"));
            return CalculateCheckCode(numberBuilder.ToString());
        }

        private static readonly int[] Weight = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
        private static readonly char[] CheckCodeMapper = new char[] { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };

    }
}
