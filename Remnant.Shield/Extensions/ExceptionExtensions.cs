using System;

namespace Remnant.Extensions
{
	public static class ExceptionExtensions
	{
		public static (bool, Type) Evaluate(this ArgumentNullException ex, object instance) => (instance == null, typeof(ArgumentNullException));
	}
}
