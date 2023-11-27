using Remnant.Exceptions;

namespace Remnant.ShieldTestArea
{
	internal class TestClient : BaseTest
	{


		[Test]
		public void BaseTest0()
		{
			// not registered,  raise with nothing specified, use standard exception message
			//e:msg:  System.Exception : Exception of type 'System.Exception' was thrown.
			Test<Exception>(() => Shield.Raise(), "Exception of type 'System.Exception' was thrown.");

			// not registered, raise with null message, use standard exception message
			//e:msg:  System.MissingMethodException : Constructor on type 'System.Exception' not found.
			Test<NullReferenceException>(() => Shield.Raise(null), "Object reference not set to an instance of an object.");

			// not registered, raise no exception with message, use specified message
			//e:msg: System.Exception: 'Unknown exception'
			Test<Exception>(() => Shield.Raise("Unknown exception"), "Unknown exception");

			// not registered, raise with exception, use exception message
			//e:msg: System.ArgumentException : Value does not fall within the expected range.
			Test<ArgumentException>(() => Shield.Raise<ArgumentException>(), "Value does not fall within the expected range.");

			// not registered, raise with exception with message, use raise message
			//e:msg: System.ArgumentException : Stop your arguments!
			Test<ArgumentException>(() => Shield.Raise<ArgumentException>("Stop your arguments!"), "Stop your arguments!");

			// not registered, raise no exception with message, use raise message (using inner exception as test)
			//e:msg:inner: System.Exception : Domain exception ---> System.AppDomainUnloadedException : Attempted to access an unloaded AppDomain.
			Test<Exception>(() => Shield.Raise("Domain exception", new AppDomainUnloadedException()), "Domain exception");

			//REGISTER:

			//standard exception, register with no message, raise exception wth no message, use exception message 
			Shield.Configure.Register<ContextMarshalException>();
			Test<ContextMarshalException>(() => Shield.Raise<ContextMarshalException>(), "Attempted to marshal an object across a context boundary.");

			//standard exception, register with no message, raise exception with message , use raise message
			Shield.Configure.Register<ContextMarshalException>();
			Test<ContextMarshalException>(() => Shield.Raise<ContextMarshalException>("Someone shot the marshal but not the deputy hehe"), "Someone shot the marshal but not the deputy hehe");

			//standard exception, register with message, raise exception with no message, use register message
			Shield.Configure.Register<AccessViolationException>("Stop violating your access.");
			Test<AccessViolationException>(() => Shield.Raise<AccessViolationException>(), "Stop violating your access.");

			//standard exception, register with message, raise exception with message, use raise message
			//System.ContextMarshalException : System.ContextMarshalException : We have encountered an out of context object.
			Shield.Configure.Register<ArithmeticException>("Calculation error.");
			Test<ArithmeticException>(() => Shield.Raise<ArithmeticException>("Dumb and dumber"), "Dumb and dumber");
		}

		[Test]
		public void BaseTest1()
		{
			//REGISTER: USING PLACEHOLDERS

			//standard exception, register with message (+ placeholders):
			Shield.Configure.Register<FileNotFoundException>("Unable to find the file {0}, you lost it?");

			//raise exception with no message, config set to fail cause args specified
			Shield.Configure.ErrorOnUnparsedMessages(true);
			Test<ShieldException>(() => Shield.Raise<FileNotFoundException>(), "The exception message 'Unable to find the file {0}, you lost it?' contains parameter placeholders but no values or the incorrect amount of values are specified.");

			//raise exception with no message, config set to replace raise message with standard exception message
			Shield.Configure
				.ErrorOnUnparsedMessages(false)
				.ReplaceOnUnparsedMessages(true);
			Test<FileNotFoundException>(() => Shield.Raise<FileNotFoundException>(), "Unable to find the specified file.");

			//raise exception with no message, cfg set not to fail or replace showing message as specified 
			Shield.Configure
				.ErrorOnUnparsedMessages(false)
				.ReplaceOnUnparsedMessages(false);
			Test<FileNotFoundException>(() => Shield.Raise<FileNotFoundException>(), "Unable to find the file {0}, you lost it?");

			//standard exception, register with message (+ placeholders), raise exception with message, use register message, use raise message as placeholder value
			Test<FileNotFoundException>(() => Shield.Raise<FileNotFoundException>("Secrets.txt"), "Unable to find the file Secrets.txt, you lost it?");

			//standard exception, register with message (NO placeholders), raise exception with message, use raise message not as placeholder value
			//System.IO.FileNotFoundException : I have lost my Secrets.txt file boohoo!
			//Shield.Configure.Register<FileNotFoundException>("Unable to find the file, you lost it?");
			//Shield.Raise<FileNotFoundException>("I have lost my Secrets.txt file boohoo!");
		}

		[Test]
		public void Must_be_able_to_handle_exceptions_from_delegate_work_and_propogate_exception()
		{
			var exception = Assert.Throws<ShieldException>(() =>
			Shield.Handle((ref string context) =>
			{
				context = "Fetch data:";

				throw new Exception("Something went wrong!");
			}, suppress: false));

			Assert.That(exception.Message == "Fetch data: Something went wrong!");
		}
		[Test]
		public void Must_be_able_to_handle_exceptions_from_delgate_work_and_suppress_exception()
		{
			var exception = Shield.Handle((ref string context) =>
			{
				context = "Fetch data:";
				throw new Exception("Something went wrong!");
			}, suppress: true);

			Assert.That(exception.Message == "Something went wrong!");
		}

		[Test]
		public void BaseTest5()
		{
			AssertException.Evaluate(1 > 0);
		}
	}
}