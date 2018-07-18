using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Changingsoft.Imaging
{
	/// <summary>
	/// 表示缩略图尺寸调整模式。
	/// </summary>
	public enum ResizeMode
	{
		/// <summary>
		/// 表示图像被拉伸填满指定的矩形区域。缩略图尺寸为指定尺寸。
		/// </summary>
		Fill,
		/// <summary>
		/// 表示缩略图应等比例缩放占据指定的矩形区域。不足的部分对称由底色留白。缩略图尺寸为指定尺寸。
		/// </summary>
		Fit,
		/// <summary>
		/// 表示缩略图应等比例缩放，占据至多矩形区域内，宽度和高度不足的按实际计算尺寸渲染。缩略图尺寸为实际计算尺寸。
		/// </summary>
		FitCut,
		/// <summary>
		/// 标识缩略图应等比例缩放占据指定的矩形区域，不进行留白，超出的部分对称截断。缩略图尺寸为指定尺寸。
		/// </summary>
		StretchFit,
		/// <summary>
		/// 表示缩略图按照指定矩形的宽度值等比例缩放，忽略高度值。缩略图尺寸为实际计算尺寸。
		/// </summary>
		ScaleX,
		/// <summary>
		/// 表示缩略图按照指定举行的高度值等比例缩放，忽略宽度值。缩略图尺寸为实际计算尺寸。
		/// </summary>
		ScaleY
	}
}
