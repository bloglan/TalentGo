using System;
using System.Collections.Specialized;
using System.Web.Routing;

namespace TalentGo.Extension
{
    public static class FormatterExtension
	{
		public static string FormatAs(this DateTime datetime, DateTimeParts parts, char DateSpraster, char TimeSp)
		{
			return string.Empty;
		}

		public static string SmartShow(this DateTime dt)
		{
			DateTime current = DateTime.Now;
			if (dt < current.AddYears(-1))
			{
				return dt.ToString("yyyy-M-d");
			}
			else if (dt < current.AddDays(-30))
			{
				return dt.ToString("M-d");
			}
			else if (dt < current.AddDays(-1))
			{
				return ((int)(current - dt).TotalDays).ToString() + "天前";
			}
			else if (dt < current.AddHours(-1))
			{
				return ((int)(current - dt).TotalHours).ToString() + "小时前";
			}
			else if (dt < current.AddMinutes(-1))
			{
				return ((int)(current - dt).TotalMinutes).ToString() + "分钟前";
			}
			else if (dt <= current)
			{
				return "刚刚";
			}
			else if (dt < current.AddMinutes(1))
			{
				return ((int)(dt - current).TotalSeconds).ToString() + "秒后";
			}
			else if (dt < current.AddHours(1))
			{
				return ((int)(dt - current).TotalMinutes).ToString() + "分钟后";
			}
			else if (dt < current.AddDays(1))
			{
				return ((int)(dt - current).TotalHours).ToString() + "小时后";
			}
			else if (dt < current.AddMonths(1))
			{
				return ((int)(dt - current).TotalDays).ToString() + "天后";
			}
			else if(dt < current.AddYears(1))
			{
				return dt.ToString("M-d");
			}
			else
			{
				return dt.ToString("yyyy-M-d");
			}
		}

		/// <summary>
		/// 计算到当前时间时的周岁年龄。
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static int Age(this DateTime date)
		{
			DateTime now = DateTime.Now;
			if (date > now)
				return 0;

			//取得生日的月和日
			int birthMonth = date.Month;
			int birthDay = date.Day;

			//计算由出生年到当前年的差再减1作为年龄基准，为周岁基准。
			int ageBase = now.Year - date.Year - 1;

			if (now.Month > birthMonth)
			{
				//当前月份大于出生月份，则基准+1
				ageBase++;
			}
			else
			{
				//否则需看日期
				if (now.Day > birthDay)
				{
					//当前日大于出生日，则基准+1
					//此处对闰年处置有效：
					//若闰年2月29日出生，当前年无论是否闰年，总在3月1日起算满周岁。
					//若平年2月28日出生，当前年为闰年时，2月29日值大于28，算满周岁。若当前年为闰年，则3月1日起算满周岁。

					ageBase++;
				}
			}

			return ageBase;
		}

		public static string ToByteSize(this int size)
		{
			int absSize = Math.Abs(size);
			if (absSize < 1024)
				return size + "Bytes";
			else if (absSize < 1024 * 1024)
				return ((decimal)size / 1024m).ToString("0.#") + "KB";
			else if (absSize < 1024 * 1024 * 1024)
				return ((decimal)size / (1024m * 1024m)).ToString("0.#") + "MB";
			else
				return ((decimal)size / (1024m * 1024m * 1024m)).ToString("0.#") + "GB";
		}

		public static RouteValueDictionary ToRouteValues(this NameValueCollection col, Object AdditionalRouteValues)
		{
			var values = new RouteValueDictionary(AdditionalRouteValues);
			if (col != null)
			{
				foreach (string key in col)
				{
					//values passed in object override those already in collection
					if (!values.ContainsKey(key)) values[key] = col[key];
				}
			}
			return values;
		}

	}

	[Flags]
	public enum DateTimeParts
	{
		Year = 1,
		Month = 2,
		Day = 4,
		Hour = 8,
		Minute = 16,
		Second = 32,
		MillionSecond = 64,
		YearAndMonth = 3,
		DateOnly = 7,
		TimeOnly = 56
	}
}
