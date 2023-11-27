using System;

namespace Remnant.Extensions
{
	public static class ObjectExtensions
	{
		public static bool IsNumber(this object source)
		{
			return source is int || source is uint || source is long || source is ulong || source is short || source is ushort ||
				source is byte || source is sbyte || source is decimal || source is double || source is float;
		}

		public static bool IsType(this object source, Type type)
		{
			return source.GetType() == type;
		}

		public static bool IsType<TType>(this object source)
		{
			return source.GetType() == typeof(TType);
		}
	}
}
