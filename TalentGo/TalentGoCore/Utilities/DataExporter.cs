using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Utilities
{
	/// <summary>
	/// 一种类型对象的CSV数据导出器。
	/// </summary>
	/// <typeparam name="T">用于导出CSV的数据类型</typeparam>
	public class DataExporter<T>
	{
		public DataExporter()
		{

		}

		T inst;

		void RendColumn(Func<T, bool> expression)
		{
			if (expression(inst))
			{
				return;
			}
		}

		protected virtual string GetColumnTitle(PropertyInfo property)
		{
			return property.Name;
		}

		protected virtual string GetColumnData(PropertyInfo property, T data)
		{
			var result = property.GetValue(data);
			if (result == null)
				return string.Empty;
			return result.ToString();
		}

		public void ExportCSV(T data, Stream stream)
		{
			this.ExportCSV(data, stream, GetColumnTitle, GetColumnData);
		}

		public void ExportCSV(T data, Stream stream, Func<PropertyInfo, string> ColumnTitleExpression, Func<PropertyInfo, T, string> ColumnDataExpression)
		{

		}
	}
}
