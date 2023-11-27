using Remnant.Exceptions;
using Remnant.Resources;
using Remnant.ShieldTestArea.TestObjects;
using System.Data.SqlTypes;

namespace Remnant.ShieldTestArea
{
	internal class TestRegisterAndRaise : BaseTest
	{
		private readonly Client _client = new Client { Balance = 10, Name = "Marisca", CreditRating = 4, Rating = 7, Status = 12.6 };

		public TestRegisterAndRaise()
		{
		}

		[Test]
		public void Must_fail_registering_exception_twice()
		{
			Shield.Configure.EnforceRegistrationCheck(true);

			var exception = Assert.Throws<ShieldException>(() =>
			{
				Shield.Configure.Register<InvalidAgeException>();
				Shield.Configure.Register<InvalidAgeException>();
			});
			;
			Assert.That(exception.Message == string.Format(Messages.AlreadyRegistered, typeof(InvalidAgeException)));
			Shield.Configure.EnforceRegistrationCheck(false);
		}

		[Test]
		public void Must_be_able_to_raise_unregistered_exception()
		{
			Test<SqlNullValueException>(() =>
				Shield.Raise<SqlNullValueException>(),
				"Data is Null. This method or property cannot be called on Null values.");
		}

		[Test]
		public void Must_be_able_to_register_exception_with_message_and_raise()
		{
			Shield.Configure.Register<ThreadInterruptedException>("Thread been interrupted whilst in wait state.");

			Test<ThreadInterruptedException>(() =>
				Shield.Raise<ThreadInterruptedException>(),
				"Thread been interrupted whilst in wait state.");
		}

		[Test]
		public void Must_be_able_to_register_exception_with_interpolated_message_and_raise()
		{
			Shield.Configure.Register<InvalidAgeException>("The age limit is {0}.");

			var age = 18;
			Test<InvalidAgeException>(() =>
				Shield
				.Against(age < 21)
				.Raise<InvalidAgeException>(21),
				"The age limit is 21.");
		}

		[Test]
		public void Must_be_able_to_register_exception_with_no_message_and_raise_with_message()
		{
			Shield.Configure.Register<AccessViolationException>();

			Test<AccessViolationException>(() =>
				Shield.Raise<AccessViolationException>("Memory is protected."),
				"Memory is protected.");
		}

		[Test]
		public void Must_be_able_to_register_exception_with_no_message_and_raise_with_no_message()
		{
			Shield.Configure.Register<AccessViolationException>();

			Test<AccessViolationException>(() =>
				Shield.Raise<AccessViolationException>(),
				"Attempted to read or write protected memory. This is often an indication that other memory is corrupt.");
		}

		[Test]
		public void Must_use_shield_exception_message_for_unregistered_exception()
		{
			Shield.Configure.DeRegister<ArgumentNullException>();

			Test<ArgumentNullException>(() =>
				Shield.Raise<ArgumentNullException>(),
				"Value cannot be null.");
		}

		[Test]
		public void Must_use_shield_exception_message_for_unregistered_exception_with_parameter()
		{
			Shield.Configure.DeRegister<ArgumentNullException>();

			Test<ArgumentNullException>(() =>
				Shield.Raise<ArgumentNullException>("some_variable"),
				"Value cannot be null. (Parameter 'some_variable')");
		}

		public void Must_use_shield_exception_message_for_registered_exception_with_parameter()
		{
			Test<ArgumentNullException>(() =>
				Shield.Raise<ArgumentNullException>("some_variable"),
				"Value cannot be null. (Parameter 'some_variable')");
		}

		[Test]
		public void Must_be_able_to_use_registered_message_for_registered_exception_with_no_default_message()
		{
			Shield.Configure.Register<NullReferenceException>("Hey stop nulling around! The culprit is '{0}'.");

			Test<NullReferenceException>(() =>
				Shield.Raise<NullReferenceException>("some_object"),
				"Hey stop nulling around! The culprit is 'some_object'.");
		}

		[Test]
		public void Must_be_able_to_use_shield_registered_exceptions_with_default_message()
		{
			Test<ArgumentNullException>(() =>
				Shield.Raise<ArgumentNullException>("some_variable"),
				"Value cannot be null. (Parameter 'some_variable')");
		}

		[Test]
		public void Must_be_able_to_register_exception_with_evaluation_call()
		{
			Shield.Configure.Register<CreditException>((p) =>
			{
				return _client.CreditRating == 4;
			}, 
			"Testing client exception evaluated failed!");

			Test<CreditException>(() =>
				Shield
					.Raise<CreditException>(),
				"Testing client exception evaluated failed!");
		}
	}
}