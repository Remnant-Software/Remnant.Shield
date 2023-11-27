using System;

namespace Remnant.Exceptions
{
	/// <summary>
	/// Exception raised when variable is null or whitespace.
	/// Default error message: The variable '{0}' is null or whitespace.
	/// </summary>
	public class NullOrWhitespaceException : ShieldException
	{
		private static new string DefaultErrorMessage = "The variable is null or contains whitespaces.";
		public static string ErrorMessage = "The variable '{0}' is null or contains whitespaces.";

		public NullOrWhitespaceException() : base(DefaultErrorMessage) { }

		public NullOrWhitespaceException(string message)
			: base(message)
		{
		}

		public NullOrWhitespaceException(string message, Exception innerException)
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
				throw new ShieldException("NullOrWhitespaceException.Evaluate: expects 1 parameter.");

			if (args[0] != null && args[0] is not string)
				throw new ShieldException("NullOrWhitespaceException.Evaluate: expects parameter of type 'string'.");

			return string.IsNullOrWhiteSpace((string)args[0]);
		}
	}
}
