using System;

namespace Remnant.Exceptions
{
	/// <summary>
	/// Exception raised when object is not of specified type.
	/// Default error message: The object '{0}' is not of the specified type {1}.
	/// </summary>
	public class InvalidTypeException : ShieldException
	{
		private static new string DefaultErrorMessage = "Invalid object type.";
		public static string ErrorMessage = "The object '{0}' is not of the specified type '{1}'.";

		public InvalidTypeException() : base(DefaultErrorMessage) { }

		public InvalidTypeException(string message)
			: base(message)
		{
		}

		public InvalidTypeException(string message, Exception innerException)
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
