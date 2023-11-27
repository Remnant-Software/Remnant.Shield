using Remnant.Extensions;
using System;

namespace Remnant.Exceptions
{
	/// <summary>
	/// Exception raised when object falls outside the minimum and maximum range.
	/// Default error message: The value for '{0}' falls outside the required minimum and maximum range (min={1}, max={2}).
	/// </summary>
	public class MinMaxRangeException : ShieldException
	{
		private static new string DefaultErrorMessage = "The value is outside the specified range.";
		public static string ErrorMessage = "The value for '{0}' falls outside the required minimum and maximum range (min={1}, max={2}).";

		public MinMaxRangeException() : base(DefaultErrorMessage) { }

		public MinMaxRangeException(string message)
			: base(message)
		{
		}

		public MinMaxRangeException(string message, Exception innerException)
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
			if (args.Length != 3)
				throw new ShieldException("MinMaxRangeException.Evaluate: expects 3 parameter2. Example: [5, 1, 10");

			if (args[0].IsNumber())
			{
				if (!args[1].IsNumber()) throw new ShieldException("MinMaxRangeException.Evaluate: expects 2nd parameter of type number.");
				if (!args[2].IsNumber()) throw new ShieldException("MinMaxRangeException.Evaluate: expects 3rd parameter of type number.");

				if (args[0].IsType<int>())
					return !((int)args[0] > (int)args[1] && (int)args[0] < (int)args[2]);

				if (args[0].IsType<long>())
					return !((long)args[0] > (long)args[1] && (long)args[0] < (long)args[2]);

				if (args[0].IsType<double>())
					return !((double)args[0] > (double)args[1] && (double)args[0] < (double)args[2]);

				if (args[0].IsType<long>())
					return !((long)args[0] > (long)args[1] && (long)args[0] < (long)args[2]);

				if (args[0].IsType<decimal>())
					return !((decimal)args[0] > (decimal)args[1] && (decimal)args[0] < (decimal)args[2]);

				if (args[0].IsType<float>())
					return !((float)args[0] > (float)args[1] && (float)args[0] < (float)args[2]);

				if (args[0].IsType<sbyte>())
					return !((sbyte)args[0] > (sbyte)args[1] && (sbyte)args[0] < (sbyte)args[2]);

				if (args[0].IsType<byte>())
					return !((byte)args[0] > (byte)args[1] && (byte)args[0] < (byte)args[2]);

				if (args[0].IsType<ushort>())
					return !((ushort)args[0] > (ushort)args[1] && (ushort)args[0] < (ushort)args[2]);

				if (args[0].IsType<short>())
					return !((short)args[0] > (short)args[1] && (short)args[0] < (short)args[2]);

				if (args[0].IsType<ulong>())
					return !((ulong)args[0] > (ulong)args[1] && (ulong)args[0] < (ulong)args[2]);

				if (args[0].IsType<uint>())
					return !((uint)args[0] > (uint)args[1] && (uint)args[0] < (uint)args[2]);
			}

			if (args[0].IsType<DateTime>())
			{
				if (!args[1].IsType<DateTime>()) throw new ShieldException("MinMaxRangeException.Evaluate: expects 2nd parameter of type 'DateTime'.");
				if (!args[2].IsType<DateTime>()) throw new ShieldException("MinMaxRangeException.Evaluate: expects 3rd parameter of type 'DateTime'.");

				return !((DateTime)args[0] > (DateTime)args[1] && (DateTime)args[0] < (DateTime)args[2]);
			}

			throw new ShieldException("MinMaxRangeException.Evaluate: does not support the parameter types.");
		}
	}
}
