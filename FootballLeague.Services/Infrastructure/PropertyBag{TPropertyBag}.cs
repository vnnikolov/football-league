using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FootballLeague.Services.Infrastructure
{
	/// <summary>
	/// Represents a property set.
	/// </summary>
	/// <typeparam name="TPropertyBag">type to wrap</typeparam>
	public abstract class PropertyBag<TPropertyBag> : PropertyBag
		where TPropertyBag : PropertyBag
	{
		/// <summary>
		/// Checks whether a property value is set or not.
		/// </summary>
		/// <typeparam name="TValue">type of value</typeparam>
		/// <param name="expr">property expression</param>
		/// <returns><value>true</value> if the value is set, otherwise - <value>false</value></returns>
		public bool IsSet<TValue>(Expression<Func<TPropertyBag, TValue>> expr)
		{
			string propertyName = GetPropertyName(expr);

			return IsSet<TValue>(propertyName);
		}

		/// <summary>
		/// Gets the metadata of a property.
		/// </summary>
		/// <typeparam name="TValue">type of value</typeparam>
		/// <param name="expr">property expression</param>
		/// <returns>property metadata</returns>
		private static string GetPropertyName<TValue>(Expression<Func<TPropertyBag, TValue>> expr)
		{
			if (expr == null)
			{
				throw new ArgumentNullException(nameof(expr));
			}

			var memberExpression = (MemberExpression)expr.Body;
			if (memberExpression.Member.MemberType != MemberTypes.Property)
			{
				throw new ArgumentException("Member is not a property.", nameof(expr));
			}

			var propertyInfo = (PropertyInfo)memberExpression.Member;

			return propertyInfo.Name;
		}
	}
}
