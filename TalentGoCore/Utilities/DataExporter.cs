using System;
using System.IO;
using System.Reflection;

namespace TalentGo.Utilities
{
    /// <summary>
    /// 一种类型对象的CSV数据导出器。
    /// </summary>
    /// <typeparam name="T">用于导出CSV的数据类型</typeparam>
    public class DataExporter<T>
	{
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
		protected virtual string GetColumnTitle(PropertyInfo property)
		{
			return property.Name;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="data"></param>
        /// <returns></returns>
		protected virtual string GetColumnData(PropertyInfo property, T data)
		{
			var result = property.GetValue(data);
			if (result == null)
				return string.Empty;
			return result.ToString();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="stream"></param>
		public void ExportCSV(T data, Stream stream)
		{
			this.ExportCSV(data, stream, GetColumnTitle, GetColumnData);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="stream"></param>
        /// <param name="ColumnTitleExpression"></param>
        /// <param name="ColumnDataExpression"></param>
		public void ExportCSV(T data, Stream stream, Func<PropertyInfo, string> ColumnTitleExpression, Func<PropertyInfo, T, string> ColumnDataExpression)
		{

		}
	}
}
