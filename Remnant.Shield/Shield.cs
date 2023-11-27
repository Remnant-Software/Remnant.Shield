using Remnant.Exceptions;
using Remnant.Interfaces;
using System;
using System.Runtime.CompilerServices;

namespace Remnant
{
	public delegate void HandleInvoke(ref string context);

	/// <summary>
	/// Shield
	/// </summary>
	public static class Shield
	{
		[ThreadStatic]
		private static IShield _innerShield;
		private static object _padLock = new();

		private static IShield Instance
		{
			get { lock (_padLock) { return _innerShield ??= InternalShield.Create(); } }
		}

		/// <summary>
		/// Configure the shield
		/// </summary>
		public static IShieldConfigure Configure => Instance.Configure;

		/// <summary>
		/// Shield will intercept any raised exception.<br/>
		/// The 'context' on the delgate will be prefixed to the exception message. 
		///	<code>Example: (suppress must be false)
		///	Shield.Handle((ref string context) =>
		///	{
		///	  context = "Fetch data:";
		///	  throw new Exception("Something went wrong!");
		///	});
		///	</code>
		///	<para> The exception message: 'Fetch data: Something went wrong!'</para>	
		/// </summary>
		/// <param name="action">Delegate for the code to be handled. The 'context' will be prefixed to exception message. </param>
		/// <param name="suppress">Propagate or suppress exception (default = false)</param>
		/// <returns>Returns exception only if suppress is true</returns>
		public static Exception Handle(HandleInvoke action, bool suppress = false) => Instance.Handle(action, suppress);

		/// <summary>
		/// Shield will intercept any raised exception.
		///	<code>Example:
		///	Shield.Handle(() =>
		///	{
		///	  throw new Exception("Something went wrong!");
		///	});
		///	</code>
		/// </summary>
		/// <param name="action">Delegate for the code to be handled</param>
		/// <param name="suppress">Propagate or suppress exception (default = false)</param>
		/// <returns>Returns exception only if suppress is true</returns>
		public static Exception Handle(Action action, bool suppress = false) => Instance.Handle(action, suppress);

		/// <summary>
		/// Raise exception with optional error message
		/// </summary>
		/// <param name="message">The error message (optional)</param>
		/// <returns>Returns the Shield interface</returns>
		public static IShield Raise(object message = null) => 
			Instance.Raise(message);

		/// <summary>
		/// Raise exception with optional error message
		/// </summary>
		/// <param name="message">The error message (optional)</param>
		/// <param name="innerException">The inner exception for this exception</param>
		/// <returns>Returns the Shield interface</returns>
		public static IShield Raise(object message, Exception innerException) =>
			Instance.Raise(message, innerException);

		/// <summary>
		/// Raise generic exception with optional error message
		/// </summary>
		/// <typeparam name="TException">The generic exception type</typeparam>
		/// <param name="message">The error message (optional)</param>
		/// <returns>Returns the Shield interface</returns>
		public static IShield Raise<TException>(object message = null) where TException : Exception => 
			Instance.Raise<TException>(message);

		/// <summary>
		/// Raise exception
		/// </summary>
		/// <param name="exception">The exception to be raised</param>
		/// <returns>Returns the Shield interface</returns>
		public static IShield Raise(Exception exception) =>
			Instance.Raise(exception);

		/// <summary>
		/// Raise generic exception with speciified inner exception and error message
		/// </summary>
		/// <typeparam name="TException">The generic exception type</typeparam>
		/// <param name="message">The error message</param>	
		/// <param name="innerException">he inner exception for this exception</param>
		/// <returns>Returns the Shield interface</returns>
		public static IShield Raise<TException>(object message, Exception innerException) where TException : Exception =>
			Instance.Raise<TException>(message, innerException);


		/// <summary>
		/// Shield against a boolean evaluated expression
		/// </summary>
		/// <param name="assertion">The assertion</param>
		/// <param name="messageParameters">Optional message parameters</param>
		/// <returns>Returns the Shield interface</returns>
		public static IShield Against(bool assertion, params object[] messageParameters) =>
			Instance.Against(assertion, messageParameters);

		/// <summary>
		/// Shield against nullable object 
		/// </summary>
		/// <param name="instance">The object instance</param>
		/// <param name = "paramName" > Name will be inferred or can be explicitly specified</param>
		/// <returns>Returns the Shield interface</returns>
		public static IShield AgainstNull(object instance, [CallerArgumentExpression("instance")] string? paramName = "instance") =>
			Instance.AgainstNull(instance, paramName);

		/// <summary>
		/// Shield against a null or empty string
		/// </summary>
		/// <param name="instance">The string instance</param>
		/// <param name = "paramName" > Name will be inferred or can be explicitly specified</param>
		/// <returns>Returns the Shield interface</returns>
		public static IShield AgainstNullOrEmpty(string instance, [CallerArgumentExpression("instance")] string? paramName = "instance") =>
			Instance.AgainstNullOrEmpty(instance, paramName);

		/// <summary>
		/// Shield against a null or whitespace string
		/// </summary>
		/// <param name="instance">The string instance</param>
		/// <param name = "paramName" > Name will be inferred or can be explicitly specified</param>
		/// <returns>Returns the Shield interface</returns>
		public static IShield AgainstNullOrWhitespace(string instance, [CallerArgumentExpression("instance")] string? paramName = "instance") =>
			Instance.AgainstNullOrWhitespace(instance, paramName);

		/// <summary>
		/// Shield against a boolean evaluated nethod expression
		/// </summary>
		/// <param name="assertion">The assertion method</param>
		/// <param name="messageParameters">Optional message parameters</param>
		/// <returns>Returns the Shield interface</returns>
		public static IShield Against(Func<bool> assertion, params object[] messageParameters) =>
			Instance.Against(assertion, messageParameters);

		/// <summary>
		/// Shield against an out of range integer value
		/// </summary>
		/// <param name="value">The value</param>
		/// <param name="min">The minimum of the value range</param>
		/// <param name="max">The maximum of the value range</param>
		/// <param name="messageParameters">Optional message parameters</param>
		/// <returns>Returns the Shield interface</returns>
		public static IShield AgainstNotInRange(int value, int min, int max, params object[] messageParameters) =>
			Instance.AgainstNotInRange(value, min, max, messageParameters);

		/// <summary>
		/// Shield against an out of range decimal value
		/// </summary>
		/// <param name="value">The value</param>
		/// <param name="min">The minimum of the value range</param>
		/// <param name="max">The maximum of the value range</param>
		/// <param name="messageParameters">Optional message parameters</param>
		/// <returns>Returns the Shield interface</returns>
		public static IShield AgainstNotInRange(decimal value, decimal min, decimal max, params object[] messageParameters) =>
			Instance.AgainstNotInRange(value, min, max, messageParameters);

		/// <summary>
		/// Shield against an out of range double value
		/// </summary>
		/// <param name="value">The value</param>
		/// <param name="min">The minimum of the value range</param>
		/// <param name="max">The maximum of the value range</param>
		/// <param name="messageParameters">Optional message parameters</param>
		/// <returns>Returns the Shield interface</returns>
		public static IShield AgainstNotInRange(double value, double min, double max, params object[] messageParameters) =>
			Instance.AgainstNotInRange(value, min, max, messageParameters);

		/// <summary>
		/// Shield against an out of range date value
		/// </summary>
		/// <param name="date">The value</param>
		/// <param name="minDate">The minimum of the date range</param>
		/// <param name="maxDate">The maximum of the date range</param>
		/// <param name="messageParameters">Optional message parameters</param>
		/// <returns>Returns the Shield interface</returns>
		public static IShield AgainstNotInRange(DateTime date, DateTime minDate, DateTime maxDate, params object[] messageParameters) =>
			Instance.AgainstNotInRange(date, minDate, maxDate, messageParameters);

		/// <summary>
		/// Shield that object is of a specific type
		/// </summary>
		/// <typeparam name="TType">The generic type</typeparam>
		/// <param name="instance">The instance to check</param>
		/// <param name="messageParameters">Optional message parameters</param>
		/// <returns>Returns the Shield interface</returns>
		public static IShield MustBeOfType<TType>(object instance, params object[] messageParameters) =>
			Instance.MustBeOfType<TType>(instance, messageParameters);
	}
}