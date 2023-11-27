using Remnant.Interfaces;
using System;

namespace Remnant.Exceptions
{
	/// <summary>
	/// Exception raised when delgation is found true
	/// Default error message: The delegate '{0}' is found true.
	/// </summary>
	public class DelegateException : ShieldException
	{
		private static new string DefaultErrorMessage = "Assert degelation evaluated to be true.";
		public static string ErrorMessage = "The delegate '{0}' is found true.";

		public DelegateException() : base(DefaultErrorMessage) { }	

		public DelegateException(string message)
			: base(message)
		{
		}

		public DelegateException(string message, Exception innerException)
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
			if (args.Length != 2)
				throw new ShieldException("InvalidTypeException.Evaluate: expects 2 parameters. Example: [2, typeof(int)]");

			if (args[1] is not Type)
				throw new ShieldException("InvalidTypeException.Evaluate: expects 2nd parameter of type 'Type'.");

			return !(args[0].GetType() == (Type)args[1]);
		}
	}
}
