namespace Remnant.ShieldTestArea.TestObjects
{
	/// <summary>
	/// Need to implement minimum these methods
	/// </summary>
	public class CreditException : Exception
	{
		public CreditException() { }	
		
		public CreditException(string message, Exception innerException)
	: base(message, innerException)
		{
		}

		public CreditException(string message)
			: base(message)
		{
		}

		public static bool Evaluate(params object[] args)
		{
			return true;
		}
	}
}
