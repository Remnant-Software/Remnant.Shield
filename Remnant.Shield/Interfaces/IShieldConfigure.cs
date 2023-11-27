using System;

namespace Remnant.Interfaces
{
	public interface IShieldConfigure
	{
		void RegisterShieldExceptions();
		void DeRegisterAllExceptions();

		/// <summary>
		/// Register an exception with message.
		/// </summary>
		/// <typeparam name="TException">The exception to register</typeparam>
		/// <param name="message">The message to display for the exception (can contain parameter placeholders, ex: {0})</param>
		/// <returns>Returns the instance of the Shield</returns>
		IShieldConfigure Register<TException>(Func<object[], bool> evaluate, string message = null) where TException : Exception;


		/// <summary>
		/// Register an exception with message.
		/// </summary>
		/// <typeparam name="TException">The exception to register</typeparam>
		/// <param name="message">The message to display for the exception (can contain parameter placeholders, ex: {0})</param>
		/// <returns>Returns the instance of the Shield</returns>
		IShieldConfigure Register<TException>(string message = null) where TException : Exception;

		/// <summary>
		/// Deregister an exception
		/// </summary>
		/// <typeparam name="TException">The exception to deregister</typeparam>
		/// <returns>Returns the instance of the Shield</returns>
		IShieldConfigure DeRegister<TException>() where TException : Exception;

		/// <summary>
		/// Shield will call your method with the exception for logging purposes
		/// </summary>
		/// <param name="callback">The callback method</param>
		/// <returns>Returns the Shield instance</returns>
		IShieldConfigure Log(Action<Exception> callback);

		/// <summary>
		/// Enforce registration process to throw exception if the exception type has already been registered
		/// (default = false)
		/// </summary>
		/// <param name="enforceCheck"></param>
		/// <returns></returns>
		IShieldConfigure EnforceRegistrationCheck(bool enforceCheck);

		/// <summary>
		/// Throw exception for messages which contain place holders {0} and there are no argument parameters specified<br/>
		/// </summary>
		/// <param name="errorOnUnparsedMessages"></param>
		/// <returns></returns>
		IShieldConfigure ErrorOnUnparsedMessages(bool errorOnUnparsedMessages);

		/// <summary>
		/// Replace messages which contain place holders {0} and there are no argument parameters specified, with standard exception message<br/>
		/// </summary>
		/// <param name="replaceOnUnparsedMessages"></param>
		/// <returns></returns>
		IShieldConfigure ReplaceOnUnparsedMessages(bool errorOnUnparsedMessages);
	}
}