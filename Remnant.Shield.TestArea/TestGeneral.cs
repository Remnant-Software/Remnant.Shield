using Remnant.Exceptions;

namespace Remnant.ShieldTestArea
{
	internal class TestGeneral : BaseTest
	{
		private readonly string _personA = "John";
		private readonly string _personB = "Jane";

		public TestGeneral()
		{
			Shield.Configure
				.ReplaceOnUnparsedMessages(true)
				.ErrorOnUnparsedMessages(false);
		}

		[Test]
		public void Must_be_able_to_raise_exception_without_checks()
		{
			Test<Exception>(() =>
				Shield.Raise(new Exception("Unknown exception...kaboom!")),
				"Unknown exception...kaboom!");
		}

		[Test]
		public void Must_be_able_to_raise_exception_without_checks_using_message_only()
		{
			Test<Exception>(() =>
				Shield.Raise("Unknown exception...kaboom!"),
				"Unknown exception...kaboom!");
		}

		[Test]
		public void Must_be_able_to_raise_any_exception_without_checks()
		{
			Test<InvalidOperationException>(() =>
				Shield.Raise<InvalidOperationException>(),
				"Operation is not valid due to the current state of the object.");
		}

		[Test]
		public void Must_be_able_to_raise_any_exception_without_checks_using_message_only()
		{
			Test<InvalidOperationException>(() =>
				Shield.Raise<InvalidOperationException>("Something bombed out!"),
				"Something bombed out!");
		}

		[Test]
		public void Must_be_able_to_include_inner_exception()
		{
			//Test<Exception>(() =>
			//	Shield
			//		.Raise("Look at inner exception", new InvalidCastException()));

			//Assert.IsNotNull(exception.InnerException);
			//Assert.That(exception.Message == "Look at inner exception");
			//Assert.That(exception.InnerException.Message == "Specified cast is not valid.");
		}

		[Test]
		public void Must_be_able_to_shield_against_invalid_type()
		{
			Test<InvalidTypeException>(() =>
				Shield
					.MustBeOfType<List<int>>(new object())
					.Raise(),
					"The object 'System.Object' is not of the specified type 'System.Collections.Generic.List`1[System.Int32]'.");
		}

		[Test]
		public void Must_be_able_to_shield_use_delegate_to_evaluate_assertion()
		{
			Test<AssertException>(() =>
				Shield
				.Against(() => { return (_personA == "John"); })
				.Raise(), "The expression evaluated to be true.");

		}

		[Test]
		public void Must_be_able_to_chain_checks_with_no_failure()
		{
			string? name = "Marisca";

			Shield
				.Against(1 > 2)
				.AgainstNullOrEmpty(name)
				.Against(3 > 4)
				.Raise();
		}

		[Test]
		public void Must_be_able_to_chain_checks_with_failure()
		{
			//note: all checks are done first and then report exception, test below will report 2nd check
			string? name = null;

			Test<NullOrEmptyException>(() => Shield
					.Against(1 > 2)
					.AgainstNullOrEmpty(name)
					.Against(3 > 4)
					.Raise(), "The variable 'name' is null or empty.");
		}

		[Test]
		public void Must_be_able_to_chain_checks_with_individual_failure_raises()
		{
			//note: each check is done first and will report exception, test below will report 3rd check
			string? name = "Marisca";

			Test<AssertException>(() =>
				Shield
					.Against(1 > 2)
					.Raise()
					.AgainstNullOrEmpty(name)
					.Raise()
					.Against(_personB == "Jane")
					.Raise(), "The expression evaluated to be true.");
		}

		[Test]
		public void Must_be_able_to_use_delegates_for_against_assert()
		{
			Test<AssertException>(() =>
				Shield
					.Against(() => { return (_personA == "John"); })
					.Raise(), "The expression evaluated to be true.");
		}


	}
}