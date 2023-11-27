using Remnant.Exceptions;

namespace Remnant.ShieldTestArea
{
	public class TestNullEmpty
	{

		[Test]
		public void Should_not_throw_exception_when_not_null()
		{
			Assert.DoesNotThrow(() => Shield.AgainstNull(new object()).Raise());
		}

		[Test]
		public void Should_not_throw_exception_when_not_null_empty_string()
		{
			Assert.DoesNotThrow(() => Shield.AgainstNullOrEmpty("I am not empty").Raise());
		}

		[Test]
		public void Should_not_throw_exception_when_not_null_whitespace_string()
		{
			Assert.DoesNotThrow(() => Shield.AgainstNullOrWhitespace("I am not empty").Raise());
		}

		[Test]
		public void Must_be_able_to_shield_against_null_string()
		{
			string? whoAmI = null;
			var exception = Assert.Throws<NullOrEmptyException>(() =>
				Shield
					.AgainstNullOrEmpty(whoAmI)
					.Raise());

			Assert.That(exception.Message == "The variable 'whoAmI' is null or empty.");
		}

		[Test]
		public void Must_be_able_to_shield_against_empty_string()
		{
			string whoAmI = string.Empty;
			var exception = Assert.Throws<NullOrEmptyException>(() =>
				Shield
					.AgainstNullOrEmpty(whoAmI)
					.Raise());

			Assert.That(exception.Message == "The variable 'whoAmI' is null or empty.");
		}

		[Test]
		public void Must_be_able_to_shield_against_null_reference_using_infer_parameter_name()
		{
			object nullObject = null;

			var exception = Assert.Throws<NullException>(() =>
				Shield
					.AgainstNull(nullObject)
					.Raise());

			Assert.That(exception.Message == "The object 'nullObject' is null.");
		}

		[Test]
		public void Must_be_able_to_shield_against_null_reference_overriding_infer_parameter_name()
		{
			object nullObject = null;

			var exception = Assert.Throws<NullException>(() =>
				Shield
					.AgainstNull(nullObject, "someObject")
					.Raise());

			Assert.That(exception.Message == "The object 'someObject' is null.");
		}

		[Test]
		public void Must_be_able_to_shield_against_null_no_whitespace_string()
		{
			string whoAmI = null;
			var exception = Assert.Throws<NullOrWhitespaceException>(() =>
				Shield
					.AgainstNullOrWhitespace(whoAmI)
					.Raise());

			Assert.That(exception.Message == "The variable 'whoAmI' is null or contains whitespaces.");
		}

		[Test]
		public void Must_be_able_to_shield_against_null_with_whitespace_string()
		{
			string whoAmI = "     ";
			var exception = Assert.Throws<NullOrWhitespaceException>(() =>
				Shield
					.AgainstNullOrWhitespace(whoAmI)
					.Raise());

			Assert.That(exception.Message == "The variable 'whoAmI' is null or contains whitespaces.");
		}
	}
}