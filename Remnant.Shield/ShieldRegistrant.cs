using Remnant.Exceptions;
using System;
using System.Reflection;

namespace Remnant
{
	internal class ShieldRegistrant
	{
		public ShieldRegistrant() { }

		public ShieldRegistrant(Type exceptionType, Func<object[], bool> evaluateThreat = null, string message = null)
		{
			if (message == null)
			{
				 message = exceptionType
					.GetField(nameof(ShieldException.ErrorMessage), BindingFlags.Public | BindingFlags.Static)
					?.GetValue(null)
					?.ToString();
			}

			ExceptionType = exceptionType;
			Evaluate = evaluateThreat;
			Message = message;
		}

		public Type ExceptionType { get; set; }
		public string Message { get; set; }

		public Func<object[], bool> Evaluate;
	}
}
