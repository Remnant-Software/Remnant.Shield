using System;
using System.Runtime.CompilerServices;

namespace Remnant.Interfaces
{
	public interface IShield
	{
		/// <summary>
		/// Configure the shield
		/// </summary>
		IShieldConfigure Configure { get; }

		/// <summary>
		/// Shield will intercept any raised exception 
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
		Exception Handle(Action action, bool suppress = false);

		/// <summary>
		/// Shield will intercept any raised exception.<br/>
		/// The 'context' on the delgate will be prefixed to the exception message. 
		///	<code>Example:
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
		Exception Handle(HandleInvoke action, bool suppress = false);


		/// <summary>
		/// Raise exception with optional error message
		/// </summary>
		/// <param name="message">The error message (optional)</param>
		/// <returns>Returns the Shield interface</returns>
		IShield Raise(object message = null);

		/// <summary>
		/// Raise exception
		/// </summary>
		/// <param name="exception">The exception to be raised</param>
		/// <returns>Returns the Shield interface</returns>
		IShield Raise(Exception exception);

		/// <summary>
		/// Raise exception with error message and inner exception
		/// </summary>
		/// <param name="message">The error message (optional)</param>
		/// <param name="innerException">The inner exception for this exception</param>
		/// <returns>Returns the Shield interface</returns>
		IShield Raise(object message, Exception innerException);

		/// <summary>
		/// Raise generic exception with optional error message
		/// </summary>
		/// <typeparam name="TException">The generic exception type</typeparam>
		/// <param name="message">The error message (optional)</param>
		/// <returns>Returns the Shield interface</returns>
		IShield Raise<TException>(object message = null) where TException : Exception;

		/// <summary>
		/// Raise generic exception with speciified inner exception and error message
		/// </summary>
		/// <typeparam name="TException">The generic exception type</typeparam>
		/// <param name="message">The error message</param>	
		/// <param name="innerException">he inner exception for this exception</param>
		/// <returns>Returns the Shield interface</returns>
		IShield Raise<TException>(object message, Exception innerException) where TException : Exception;

		/// <summary>
		/// Shield against a boolean evaluated expression
		/// </summary>
		/// <param name="assertion">The assertion</param>
		/// <param name="messageParameters">Optional message parameters</param>
		/// <returns>Returns the Shield interface</returns>
		IShield Against(bool assertion, params object[] messageParameters);

		/// <summary>
		/// Shield against nullable object 
		/// </summary>
		/// <param name="instance">The object instance</param>
		/// <param name = "paramName" > Name will be inferred or can be explicitly specified</param>
		/// <returns>Returns the Shield interface</returns>
		IShield AgainstNull(object instance, [CallerArgumentExpression("instance")] string? paramName = "instance");

		/// <summary>
		/// Shield against a boolean evaluated nethod expression
		/// </summary>
		/// <param name="assertion">The assertion method</param>
		/// <param name="messageParameters">Optional message parameters</param>
		/// <returns>Returns the Shield interface</returns>
		IShield Against(Func<bool> assertion, params object[] messageParameters);

		/// <summary>
		/// Shield against a null or empty string
		/// </summary>
		/// <param name="instance">The string instance</param>
		/// <param name = "paramName" > Name will be inferred or can be explicitly specified</param>
		/// <returns>Returns the Shield interface</returns>
		IShield AgainstNullOrEmpty(string instance, [CallerArgumentExpression("instance")] string? paramName = "instance");

		/// <summary>
		/// Shield against a null or whitespace string
		/// </summary>
		/// <param name="instance">The string instance</param>
		/// <param name = "paramName" > Name will be inferred or can be explicitly specified</param>
		/// <returns>Returns the Shield interface</returns>
		IShield AgainstNullOrWhitespace(string instance, [CallerArgumentExpression("instance")] string? paramName = "instance");

		/// <summary>
		/// Shield against an out of range integer value
		/// </summary>
		/// <param name="value">The value</param>
		/// <param name="min">The minimum of the value range</param>
		/// <param name="max">The maximum of the value range</param>
		/// <param name="messageParameters">Optional message parameters</param>
		/// <returns>Returns the Shield interface</returns>
		IShield AgainstNotInRange(int value, int min, int max, params object[] messageParameters);

		/// <summary>
		/// Shield against an out of range decimal value
		/// </summary>
		/// <param name="value">The value</param>
		/// <param name="min">The minimum of the value range</param>
		/// <param name="max">The maximum of the value range</param>
		/// <param name="messageParameters">Optional message parameters</param>
		/// <returns>Returns the Shield interface</returns>
		IShield AgainstNotInRange(decimal value, decimal min, decimal max, params object[] messageParameters);

		/// <summary>
		/// Shield against an out of range double value
		/// </summary>
		/// <param name="value">The value</param>
		/// <param name="min">The minimum of the value range</param>
		/// <param name="max">The maximum of the value range</param>
		/// <param name="messageParameters">Optional message parameters</param>
		/// <returns>Returns the Shield interface</returns>
		IShield AgainstNotInRange(double value, double min, double max, params object[] messageParameters);

		/// <summary>
		/// Shield against an out of range date value
		/// </summary>
		/// <param name="date">The value</param>
		/// <param name="minDate">The minimum of the date range</param>
		/// <param name="maxDate">The maximum of the date range</param>
		/// <param name="messageParameters">Optional message parameters</param>
		/// <returns>Returns the Shield interface</returns>
		IShield AgainstNotInRange(DateTime date, DateTime minDate, DateTime maxDate, params object[] messageParameters);

		/// <summary>
		/// Shield that object is of a specific type
		/// </summary>
		/// <typeparam name="TType">The generic type</typeparam>
		/// <param name="instance">The instance to check</param>
		/// <param name="messageParameters">Optional message parameters</param>
		/// <returns>Returns the Shield interface</returns>
		IShield MustBeOfType<TType>(object instance, params object[] messageParameters);
	}
}
