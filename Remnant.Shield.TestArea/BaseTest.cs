using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remnant.ShieldTestArea
{
	internal class BaseTest
	{
		public static bool Test<TException>(Action test, string exptectedMessage)
		{
			return Test(test, typeof(TException), exptectedMessage);
		}

		public static bool Test(Action test, Type exptectedException, string exptectedMessage)
		{
			string message = string.Empty;

			try { test?.Invoke(); return true; }
			catch (Exception e)
			{
				if (e.GetType() != exptectedException)
					message = $"Unexpected exception: {e.GetType().FullName} | Expected message: {exptectedException.FullName}\n";

				if (e.Message != exptectedMessage)
					message = $"Unexpected message :{e.Message} | Expected message: {exptectedMessage}";

				if (message != string.Empty)
					throw new Exception(message);

				return true;
			}
		}
	}
}
