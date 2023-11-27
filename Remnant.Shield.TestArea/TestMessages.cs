using Remnant.Exceptions;

namespace Remnant.ShieldTestArea
{
	internal class TestMessages : BaseTest
	{
		private readonly string _personA = "John";
		private readonly string _personB = "Jane";

		public TestMessages()
		{
			//Shield.Configure.DeRegisterAllExceptions();
			//Shield.Configure.RegisterShieldExceptions();
		}

		[Test]
		public void Must_be_able_to_raise_and_get_standard_message()
		{
			Test<AssertException>(() =>
				Shield
					.Against(_personA != _personB)
					.Raise(), "The expression evaluated to be true.");
		}

		[Test]
		public void Must_be_able_to_raise_any_exception_thus_overriding_the_underlying_exception()
		{
			Test<ApplicationException>(() =>
				Shield
					.Against(_personA != _personB)
					.Raise<ApplicationException>(), "Error in the application.");
		}

		[Test]
		public void Must_be_able_to_raise_and_specify_custom_message()
		{
			Test<AssertException>(() =>
				Shield
					.Against(_personA != _personB)
					.Raise($"'{_personA} is not {_personB}'"),
					"'John is not Jane'");
		}

		[Test]
		public void Must_be_able_to_raise_and_specify_custom_message_with_placeholders_and_params()
		{
			Test<AssertException>(() =>
				Shield
					.Against(_personA != _personB, "Result:", "identical twins")
					.Raise("{0} John is not Jane, although they are {1}."),
					"Result: John is not Jane, although they are identical twins.");
		}

		[Test]
		public void Must_be_able_to_raise_with_placeholders_using_already_interpolation_message()
		{
			//note: you have to escape placeholder with double curly brackets: {{?}}
			Test<AssertException>(() =>
				Shield
					.Against(_personA != _personB, "Result:", _personB, "his sister.")
					.Raise($"{{0}} {_personA} is not {_personB}. {{1}} is {{2}}"),
					"Result: John is not Jane. Jane is his sister.");
		}

		[Test]
		public void Must_be_able_to_chain_checks_with_different_exceptions_but_using_same_message()
		{
			string? name = null;

			Test<ShieldException>(() =>
				Shield
					.Against(1 < 0)
					.AgainstNullOrEmpty(name)
					.Against(3 > 4)
					.Raise<ShieldException>(), "The variable 'name' is null or empty.");
		}

		[Test]
		public void Must_be_able_to_chain_checks_with_same_exception_and_using_same_message()
		{
			string? name = null;

			Test<ShieldException>(() => Shield
					.Against(1 > 0)
					.AgainstNullOrEmpty(name)
					.Against(3 > 4)
					.Raise<ShieldException>("Something failed!"),
					"Something failed!");
		}

		[Test]
		public void Must_be_able_to_chain_checks_with_same_exception_and_using_different_messages()
		{
			string? name = null;

			try
			{
				Shield
					.Against(1 < 0)
					.Raise("One is less than zero")
					.AgainstNullOrEmpty(name)
					.Raise("The variable '{0}' is null or empty. Press F1 for help.")
					.Against(3 > 4)
					.Raise("Three is greater than four");
			}
			catch (AssertException exception)
			{
				// none of the Against above are true
			}
			catch (NullOrEmptyException exception)
			{
				Assert.That(exception.Message == "The variable 'name' is null or empty. Press F1 for help.");
			}
		}


		//[Test]
		//public void Test_multi_tasks_threading_for_any_issues()
		//{
		//	var tasks = new List<Task>();

		//	for (var i = 0; i < 100; i++)
		//		tasks.Add(new Task(() => Must_be_able_to_include_inner_exception()));

		//	tasks.ForEach(t => t.Start());

		//	Task.WaitAll(tasks.ToArray());
		//}

		//[Test]
		//public void Must_be_able_to_log_exceptions()
		//{
		//	Shield.Configure.Log(exception =>
		//	{
		//		Assert.That(exception.Message == "The object 'System.Object' is not of the specified type System.Collections.Generic.List`1[System.Int32].");
		//	});

		//	Assert.Throws<InvalidTypeException>(() =>
		//		Shield
		//			.MustBeOfType<List<int>>(new object())
		//			.Raise<InvalidTypeException>());

		//	Shield.Configure.Log(null);
		//}
		//}
	}
}