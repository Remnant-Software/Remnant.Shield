using System;

namespace Remnant.Exceptions
{
	/// <summary>
	/// Exception raised when assertion is true
	/// Default error message: The expression {0} evaluated to be true.
	/// </summary>
	public class AssertException : ShieldException
	{
		private static new string DefaultErrorMessage = "The expression evaluated to be true.";
		public static string ErrorMessage = "The expression {0} evaluated to be true.";

		public AssertException() : base(DefaultErrorMessage)
		{
		}

		public AssertException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public AssertException(string message)
			: base(message)
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
				throw new ShieldException("AssertException.Evaluate: expects only 1 parameter.");

			if (args[0] is not bool && args[0] is not MulticastDelegate)
				throw new ShieldException("AssertException.Evaluate: expects parameter of type 'bool' or a delegate.");

			if (args[0] is MulticastDelegate)
				return (bool)((MulticastDelegate)args[0])?.DynamicInvoke();
			else
				return (bool)args[0];
		}
	}
}
