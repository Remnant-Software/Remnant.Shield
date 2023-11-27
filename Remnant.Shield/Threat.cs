using Remnant.Exceptions;
using Remnant.Extensions;
using Remnant.Resources;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Remnant
{
	internal class Threat
	{
		private ShieldRegistrant _registrant;
		private bool _errorOnUnparsedMessages;
		private bool _replaceOnUnparsedMessages;
		private object _raiseMessage;
		private bool _override;

		internal static Threat Create()
		{
			return new Threat();
		}

		private Threat() { }

		public Type RaiseExceptionType { get; private set; }
		public bool IsCritical { get; private set; }
		public Action<Exception> Callback { get; private set; }
		public Exception InnerException { get; private set; }
		public List<object> Arguments { get; private set; } = new();

		public Threat AssignRegistrant(ShieldRegistrant registrant)
		{
			_registrant = registrant;
			return this;
		}

		public Threat UseRaiseException(Type exception, object message)
		{
			RaiseExceptionType = exception;
			_raiseMessage = message;
			return this;
		}

		public Threat Override(Type exception)
		{
			_override = true;
			RaiseExceptionType = exception;
			return this;
		}

		public Threat Override(object message, Type exception = null)
		{
			_override = true;
			_registrant = null;
			_raiseMessage = message;
			if (exception != null)
				RaiseExceptionType = exception;
			return this;
		}

		public Threat AddPlaceValues(List<object> arguments)
		{
			Arguments.AddRange(arguments);
			return this;
		}

		public Threat PerformCallback(Action<Exception> callback)
		{
			Callback = callback;
			return this;
		}

		public Threat WithStatus(bool isCritical)
		{
			IsCritical = isCritical;
			return this;
		}

		public Threat ErrorOnUnparsedMessages(bool errorOnUnparsedMessages)
		{
			_errorOnUnparsedMessages = errorOnUnparsedMessages;
			return this;
		}

		public Threat ReplaceOnUnparsedMessages(bool replaceOnUnparsedMessages)
		{
			_replaceOnUnparsedMessages = replaceOnUnparsedMessages;
			return this;
		}

		public Threat AssignInnerException(Exception exception)
		{
			InnerException = exception;
			return this;
		}

		public string? Message()
		{
			var message = _override ? _raiseMessage?.ToString() : _raiseMessage?.ToString() ?? _registrant?.Message?.ToString() ?? null;
			var registrantMessagePlaceHolders = _registrant?.Message != null ? _registrant.Message.CountPlaceHolders() : 0;
			var raiseMessagePlaceHolders = _raiseMessage != null ? _raiseMessage.ToString().CountPlaceHolders() : 0;

			if (registrantMessagePlaceHolders > 0 &&
				raiseMessagePlaceHolders == 0 &&
				_raiseMessage != null)
				Arguments.Add(_raiseMessage);

			if (raiseMessagePlaceHolders > 0 && Arguments.Count != raiseMessagePlaceHolders)
			{
				if (_errorOnUnparsedMessages)
					throw new ShieldException(string.Format(Messages.ParameterCountMismatch, _raiseMessage));

				if (_replaceOnUnparsedMessages)
				{
					_raiseMessage = null;
					message = _registrant?.Message ?? null;
					registrantMessagePlaceHolders = 0;
					raiseMessagePlaceHolders = 0;
				}
			}

			if (registrantMessagePlaceHolders > 0 && raiseMessagePlaceHolders == 0)
			{
				if (_errorOnUnparsedMessages)
					throw new ShieldException(string.Format(Messages.ParameterCountMismatch, message));

				if (_replaceOnUnparsedMessages && Arguments.Count == 0)
					message = null;
				else
					message = _registrant?.Message;
			}

			var totalMessagePlaceHolders = message.CountPlaceHolders();

			if (totalMessagePlaceHolders > 0 && Arguments.Count > 0)
			{
				if (Arguments.Count != totalMessagePlaceHolders)
				{
					message = Regex.Replace(message, "{.*?}", string.Empty);
					message = Regex.Replace(message, @"\s+", " ");
				}
				return string.Format(message, [.. Arguments]);
			}

			return _raiseMessage?.ToString() ?? message?.ToString() ?? null;
		}
	}
}
