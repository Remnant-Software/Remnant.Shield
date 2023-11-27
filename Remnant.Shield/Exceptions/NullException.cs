using System;

namespace Remnant.Exceptions
{
	/// <summary>
	/// Exception raised when object is null.
	/// Default error message: The object '{0}' is null.
	/// </summary>
	public class NullException : ShieldException
	{
		private static new string DefaultErrorMessage = "The object is null.";
		public static string ErrorMessage = "The object '{0}' is null.";

		public NullException() : base(DefaultErrorMessage) { }

		public NullException(string message)
			: base(message)
		{
		}

		public NullException(string message, Exception innerException)
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
				throw new ShieldException("NullException.Evaluate: expects 1 parameter.");

			return args[0] == null;
		}
	}
}
