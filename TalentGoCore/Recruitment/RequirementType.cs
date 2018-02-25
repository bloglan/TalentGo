using System.ComponentModel.DataAnnotations;

namespace TalentGo.Recruitment
{
    /// <summary>
    /// 材料的需求要求。
    /// </summary>
    public enum RequirementType
	{
		/// <summary>
		/// 零个或一个
		/// </summary>
		[Display(Name = "零个或一个")]
		ZeroOrOne = 0,
		/// <summary>
		/// 零个或多个（包括一个，允许多个）
		/// </summary>
		[Display(Name = "零个或多个")]
		ZeroOrMore = 1,
		/// <summary>
		/// 只允许一个
		/// </summary>
		[Display(Name = "一个")]
		One = 2,
		/// <summary>
		/// 一个或多个（至少一个）
		/// </summary>
		[Display(Name = "一个或多个")]
		OneOrMore = 3
	}

    /// <summary>
    /// 
    /// </summary>
	public static class RequirementTypeExtensions
	{
		static readonly int Requried = 2;
		static readonly int Multiple = 1;

		/// <summary>
		/// 获取一个值，指示该设定下要求是“需要的”。
		/// </summary>
		/// <param name="requirements"></param>
		/// <returns></returns>
		public static bool IsRequried(this RequirementType requirements)
		{
			int val = (int)requirements;
			return ((val & Requried) == Requried);
		}

		/// <summary>
		/// 获取一个值，
		/// </summary>
		/// <param name="requirements"></param>
		/// <returns></returns>
		public static bool IsMultipleEnabled(this RequirementType requirements)
		{
			int val = (int)requirements;
			return ((val & Multiple) == Multiple);
		}
	}
}
