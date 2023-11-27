using Remnant.Exceptions;

namespace Remnant.ShieldTestArea.TestObjects
{
	internal class InvalidAgeException : ShieldException
	{
		public InvalidAgeException(string message) : base(message)
		{
		}

		public InvalidAgeException(string message, Exception? innerException) : base(message, innerException)
		{
		}
	}
}
