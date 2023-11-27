using Remnant.Interfaces;
using System;

namespace Remnant.Exceptions
{
	/// <summary>
	/// Base shield exception
	/// Default error message: Unknown exception occurred.
	/// </summary>
	public class ShieldException : Exception, IShieldThreat
	{
		private static string DefaultErrorMessage = "Unknown exception occurred.";
		public static  string ErrorMessage = "Unknown exception occurred.";

		public ShieldException() : base(DefaultErrorMessage) { }

		public ShieldException(string message)
			: base(message)
		{
		}

		public ShieldException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public virtual bool Evaluate(params object[] args)
		{
			throw new NotImplementedException();
		}
	}
}
