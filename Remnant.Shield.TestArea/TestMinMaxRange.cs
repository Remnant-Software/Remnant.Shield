using Remnant.Exceptions;

namespace Remnant.ShieldTestArea
{
	public class TestMinMaxRange
	{
		[Test]
		public void Should_not_throw_exception_when_int_in_range()
		{
			Assert.DoesNotThrow(() => Shield.AgainstNotInRange(5, 1, 10).Raise());
		}

		[Test]
		public void Should_not_throw_exception_when_double_in_range()
		{
			Assert.DoesNotThrow(() => Shield.AgainstNotInRange(5.5d, 1.1d, 10.1d).Raise());
		}

		[Test]
		public void Should_not_throw_exception_when_decimal_in_range()
		{
			Assert.DoesNotThrow(() => Shield.AgainstNotInRange(15.5m, 11.1m, 20.7m).Raise());
		}

		[Test]
		public void Should_not_throw_exception_when_date_in_range()
		{
			Assert.DoesNotThrow(() => Shield.AgainstNotInRange(new DateTime(2001, 12, 30), new DateTime(2001, 12, 29), new DateTime(2001, 12, 31)).Raise());
		}

		[Test]
		public void Must_be_able_to_shield_against_int_not_in_range()
		{
			var exception = Assert.Throws<MinMaxRangeException>(() =>
				Shield
					.AgainstNotInRange(101, 80, 100)
					.Raise());

			Assert.That(exception.Message == "The value for '101' falls outside the required minimum and maximum range (min=80, max=100).");
		}

		[Test]
		public void Must_be_able_to_shield_against_date_not_in_range()
		{
			var exception = Assert.Throws<MinMaxRangeException>(() =>
				Shield
					.AgainstNotInRange(new DateTime(2000, 12, 31), new DateTime(2001, 3, 11), new DateTime(2001, 12, 31))
					.Raise());

			Assert.That(exception.Message == "The value for '12/31/2000 12:00:00 AM' falls outside the required minimum and maximum range (min=3/11/2001 12:00:00 AM, max=12/31/2001 12:00:00 AM).");
		}

		[Test]
		public void Must_be_able_to_shield_against_decimal_not_in_range()
		{
			var exception = Assert.Throws<MinMaxRangeException>(() =>
				Shield
					.AgainstNotInRange(101.04m, 80.1m, 101.03m)
					.Raise());

			Assert.That(exception.Message == "The value for '101.04' falls outside the required minimum and maximum range (min=80.1, max=101.03).");
		}

		[Test]
		public void Must_be_able_to_shield_against_doublel_not_in_range()
		{
			var exception = Assert.Throws<MinMaxRangeException>(() =>
				Shield
					.AgainstNotInRange(101.04d, 80.1d, 101.03d)
					.Raise());

			Assert.That(exception.Message == "The value for '101.04' falls outside the required minimum and maximum range (min=80.1, max=101.03).");
		}
	}
}