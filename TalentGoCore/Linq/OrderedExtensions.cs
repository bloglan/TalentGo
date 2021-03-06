﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TalentGo.Linq
{
    /// <summary>
    /// 定义一系列关于排序的扩展方法，这些方法允许使用字符串指示的字段名称作为排序字段，而不是使用lamda表达式。
    /// </summary>
	public static class OrderedExtensions
	{
		/// <summary>
		/// 使用给定实体的字段名称字符串进行排序，排序顺序为正序。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "OrderBy");
		}

		/// <summary>
		/// 使用给定的实体的字段名称字符串进行排序，排序顺序为倒序。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "OrderByDescending");
		}

		/// <summary>
		/// 将给定实体的字段名称字符串作为后续排序添加到排序表达式中，排序顺序为正序。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "ThenBy");
		}

		/// <summary>
		/// 将给定实体的字段名称字符串作为后续排序添加到排序表达式中，排序顺序为正序。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "ThenByDescending");
		}

		static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
		{
			string[] props = property.Split('.');
			Type type = typeof(T);
			ParameterExpression arg = Expression.Parameter(type, "x");
			Expression expr = arg;
			foreach (string prop in props)
			{
				// use reflection (not ComponentModel) to mirror LINQ 
				PropertyInfo pi = type.GetProperty(prop);
				expr = Expression.Property(expr, pi);
				type = pi.PropertyType;
			}
			Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
			LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

			object result = typeof(Queryable).GetMethods().Single(method => method.Name == methodName
							&& method.IsGenericMethodDefinition
							&& method.GetGenericArguments().Length == 2
							&& method.GetParameters().Length == 2)
							.MakeGenericMethod(typeof(T), type)
							.Invoke(null, new object[] { source, lambda });
			return (IOrderedQueryable<T>)result;
		}

	}
}
