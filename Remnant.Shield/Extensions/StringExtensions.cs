using System.Linq;
using System.Text.RegularExpressions;

namespace Remnant.Extensions
{
	public static class StringExtensions
	{
		public static int CountPlaceHolders(this string source)
		{
			return source == null ? 0 : Regex.Matches(source.Replace("{{", string.Empty), @"\{(\d+)")
					.OfType<Match>()
					.Select(match => int.Parse(match.Groups[1].Value))
					.Union(Enumerable.Repeat(-1, 1))
					.Max() + 1;
		}
	}
}
