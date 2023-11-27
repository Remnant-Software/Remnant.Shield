using Remnant.Exceptions;
using Remnant.Interfaces;
using Remnant.Resources;
using System;
using System.Collections.Generic;

namespace Remnant
{
	internal class InternalShield : IShield, IShieldConfigure
	{
		private readonly List<Threat> _threats = new();

		private readonly Dictionary<Type, ShieldRegistrant> _registry = [];
		private object _padLock = new();
		private Action<Exception> _logCallback;
		private bool _enforceRegistrationCheck;
		private bool _errorOnUnparsedMessages;
		private bool _replaceOnUnparsedMessages = true;

		public static IShield Create() => new InternalShield();

		private InternalShield()
		{
			RegisterShieldExceptions();
		}

		public IShieldConfigure Configure => this;

		public void RegisterShieldExceptions()
		{
			Register<AssertException>(eval => AssertException.Evaluate(eval));
			Register<NullException>(eval => NullException.Evaluate(eval));
			Register<NullOrEmptyException>(eval => NullOrEmptyException.Evaluate(eval));
			Register<NullOrWhitespaceException>(eval => NullOrWhitespaceException.Evaluate(eval));
			Register<MinMaxRangeException>(eval => MinMaxRangeException.Evaluate(eval));
			Register<InvalidTypeException>(eval => InvalidTypeException.Evaluate(eval));

			Register<ShieldException>();
			Register<ArgumentNullException>();
			Register<ArgumentOutOfRangeException>();
		}

		public void DeRegisterAllExceptions()
		{
			lock (_padLock) _registry.Clear();
		}
		public IShieldConfigure EnforceRegistrationCheck(bool enforceCheck)
		{
			_enforceRegistrationCheck = enforceCheck;
			return this;
		}

		public IShieldConfigure ReplaceOnUnparsedMessages(bool replaceOnUnparsedMessages)
		{
			_replaceOnUnparsedMessages = replaceOnUnparsedMessages;
			return this;
		}

		public IShieldConfigure ErrorOnUnparsedMessages(bool errorOnUnparsedMessages)
		{
			_errorOnUnparsedMessages = errorOnUnparsedMessages;
			return this;
		}

		public (bool, ShieldRegistrant) IsRegistered<TException>()
		{
			return (_registry.TryGetValue(typeof(TException), out ShieldRegistrant registrant), registrant);
		}

		public IShieldConfigure Register<TException>(Func<object[], bool> evaluate, string message = null) where TException : Exception
		{
			var exceptionType = typeof(TException);
			var alreadyRegistered = _registry.ContainsKey(exceptionType);

			if (_enforceRegistrationCheck && alreadyRegistered)
				throw new ShieldException(string.Format(Messages.AlreadyRegistered, exceptionType));

			var evaluateMethod = exceptionType.GetMethod("Evaluate");

			if (evaluateMethod == null)
				throw new ShieldException(string.Format(Messages.MustImplementEvaluate, exceptionType.FullName));

			if (!alreadyRegistered)
				_registry.Add(exceptionType, new ShieldRegistrant(exceptionType, evaluate, message));
			else
				_registry[exceptionType].Message = message;

			//_registry
			//	.If(!alreadyRegistered)
			//	.Add(exceptionType, new ShieldRegistrant(exceptionType, evaluate, message));

			return this;
		}
		public IShieldConfigure Register<TException>(string message = null) where TException : Exception
		{
			var exceptionType = typeof(TException);
			var alreadyRegistered = _registry.ContainsKey(exceptionType);

			if (_enforceRegistrationCheck && alreadyRegistered)
				throw new ShieldException(string.Format(Messages.AlreadyRegistered, exceptionType));

			if (!alreadyRegistered)
				_registry.Add(exceptionType, new ShieldRegistrant(exceptionType, null, message));
			else
				_registry[exceptionType].Message = message;

			//_registry
			//	.If(!alreadyRegistered)
			//		.Add(exceptionType, new ShieldRegistrant(exceptionType, null, message));
			//.Else()
			//	.Remove(exceptionType)
			//	.Add(exceptionType, new ShieldRegistrant(exceptionType, null, message));

			return this;
		}

		public IShieldConfigure DeRegister<TException>() where TException : Exception
		{
			if (_registry.ContainsKey(typeof(TException)))
				_registry.Remove(typeof(TException));

			return this;
		}

		public IShieldConfigure Log(Action<Exception> log)
		{
			_logCallback = log;
			return this;
		}

		IShieldConfigure IShieldConfigure.DeRegister<TException>()
		{
			lock (_padLock)
			{
				if (_registry.ContainsKey(typeof(TException)))
					_registry.Remove(typeof(TException));

				return this;
			}
		}

		public Exception Handle(HandleInvoke action, bool suppress = false)
		{
			var context = "Shield:";

			try
			{
				action?.Invoke(ref context);
			}
			catch (Exception e)
			{
				if (!suppress) Raise<ShieldException>($"{context} {e.Message}".Trim(), e);
				return e;
			}
			return null;
		}

		public Exception Handle(Action action, bool suppress = false)
		{
			try
			{
				action?.Invoke();
			}
			catch (Exception e)
			{
				if (!suppress) Raise<ShieldException>(e.Message, e);
				return e;
			}
			return null;
		}

		#region Assertions

		IShield IShield.Against(bool assertion, params object[] messageParameters)
		{
			CreateThreat<AssertException>([assertion])
				.AddPlaceValues([.. messageParameters]);
			return this;
		}

		IShield IShield.AgainstNull(object instance, string paramName)
		{
			CreateThreat<NullException>([instance])
				.AddPlaceValues([paramName]);
			return this;
		}

		IShield IShield.Against(Func<bool> assertion, params object[] messageParameters)
		{
			CreateThreat<AssertException>([assertion])
				.AddPlaceValues([.. messageParameters]);
			return this;
		}

		IShield IShield.AgainstNullOrEmpty(string instance, string paramName)
		{
			CreateThreat<NullOrEmptyException>([instance])
				.AddPlaceValues([paramName]);
			return this;
		}

		IShield IShield.AgainstNullOrWhitespace(string instance, string paramName)
		{
			CreateThreat<NullOrWhitespaceException>([instance])
				.AddPlaceValues([paramName]);
			return this;
		}

		IShield IShield.AgainstNotInRange(int value, int min, int max, params object[] messageParameters)
		{
			CreateThreat<MinMaxRangeException>([value, min, max])
				.AddPlaceValues([value, min, max]);
			return this;
		}

		IShield IShield.AgainstNotInRange(decimal value, decimal min, decimal max, params object[] messageParameters)
		{
			CreateThreat<MinMaxRangeException>([value, min, max])
				.AddPlaceValues([value, min, max]);
			return this;
		}

		IShield IShield.AgainstNotInRange(double value, double min, double max, params object[] messageParameters)
		{
			CreateThreat<MinMaxRangeException>([value, min, max])
				.AddPlaceValues([value, min, max]);
			return this;
		}

		IShield IShield.AgainstNotInRange(DateTime date, DateTime minDate, DateTime maxDate, params object[] messageParameters)
		{
			CreateThreat<MinMaxRangeException>([date, minDate, maxDate])
				.AddPlaceValues([date, minDate, maxDate]);
			return this;
		}

		IShield IShield.MustBeOfType<TType>(object instance, params object[] messageParameters)
		{
			CreateThreat<InvalidTypeException>([instance, typeof(TType)])
				.AddPlaceValues([instance.GetType(), typeof(TType)]);
			return this;
		}

		#endregion

		public IShield Raise(Exception exception) => throw exception;

		public IShield Raise(object message) => Raise(message, null);

		public IShield Raise<TException>(object message) where TException : Exception => Raise<TException>(message, null);

		public IShield Raise(object message, Exception innerException)
		{
			if (_threats.Count != 0)
			{
				if (message != null)
					_threats.ForEach(threat => threat.Override(message, null));
			}
			else
				CreateThreat<Exception>(null, isCritical: true, message, null, innerException);

			RaiseShield();
			return this;
		}

		public IShield Raise<TException>(object message, Exception innerException) where TException : Exception
		{
			if (_threats.Count != 0)
			{
				if (message != null)
					_threats.ForEach(threat =>
					{
						threat.Override(message, typeof(TException));
						var (isRegistered, registrant) = IsRegistered<TException>();
						if (isRegistered) 
							threat.AssignRegistrant(registrant);
						else 
							threat.Override(message, typeof(TException));	
					});
				else
					_threats.ForEach(threat => threat.Override(typeof(TException)));
			}
			else
				CreateThreat<TException>(parameters: null, isCritical: true, message, paramName: null, innerException);

			RaiseShield();
			return this;
		}

		private void RaiseShield()
		{
			try
			{
				_threats.ForEach(threat =>
				{
					if (threat.IsCritical)
					{
						Exception exception = null;
						var message = threat.Message();
						var raiseExceptionType = threat.RaiseExceptionType;
						var parameters = new List<object>();

						if (!string.IsNullOrEmpty(message))
							parameters.Add(message);	

						if (threat.InnerException != null)
							parameters.Add(threat.InnerException);

						if (threat.RaiseExceptionType != null)
							if (message == null)
								exception = (Exception)Activator.CreateInstance(raiseExceptionType);
							else
								exception = (Exception)Activator.CreateInstance(raiseExceptionType, message);
						else
							exception = (Exception)Activator.CreateInstance(raiseExceptionType, parameters.ToArray());

						_logCallback?.Invoke(exception);

						throw exception;
					}
				});
			}
			finally
			{
				_threats.Clear();
			}
		}

		private Threat CreateThreat<TException>(object[] parameters, bool isCritical = false, object message = null, string paramName = null, Exception innerException = null)
			where TException : Exception
		{
			var exceptionType = typeof(TException);
			var registrant = _registry.ContainsKey(exceptionType) ? _registry[exceptionType] : null;

			var threat = Threat
				.Create()
				.AssignRegistrant(registrant)
				.AssignInnerException(innerException)
				.ErrorOnUnparsedMessages(_errorOnUnparsedMessages)
				.ReplaceOnUnparsedMessages(_replaceOnUnparsedMessages)
				.UseRaiseException(exceptionType, message)
				.WithStatus(isCritical);

			if (registrant != null)
			{
				if (registrant.Evaluate != null)
					threat.WithStatus(registrant.Evaluate(parameters));
			}

			_threats.Add(threat);

			return threat;
		}
	}
}
