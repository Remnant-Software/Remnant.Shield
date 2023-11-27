using Remnant.Interfaces;
using System;

namespace Remnant.Exceptions
{
	/// <summary>
	/// Exception raised when string is null or empty.
	/// Default error message: The variable '{0}' is null or empty.
	/// </summary>
	public class NullOrEmptyException : ShieldException
	{
		private static new string DefaultErrorMessage = "The variable is null or empty.";
		public static string ErrorMessage = "The variable '{0}' is null or empty.";

		public NullOrEmptyException() :base(DefaultErrorMessage) { }

		public NullOrEmptyException(string message)
			: base(message)
   	{   		
   	}

		public NullOrEmptyException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Do NOT call Shield in this method (will cause recursy with stack overflow)
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// <exception cref="ShieldException"></exception>
		public new static bool Evaluate(params object[] args)
		{
			if (args.Length != 1)
				throw new ShieldException("NullOrEmptyException.Evaluate: expects 1 parameter.");

			if (args[0] != null && args[0] is not string)
				throw new ShieldException("NullOrEmptyException.Evaluate: expects parameter of type 'string'.");

			return string.IsNullOrEmpty((string)args[0]);
		}
	}
}
