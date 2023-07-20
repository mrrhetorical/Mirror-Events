using System.Collections.Generic;

namespace Calroot.MirrorEvents.Util {
	public static class EnumerableExtensions {
		public static string Stringify(this IEnumerable<object> source) {
			return $"[{string.Join(", ", source)}]";
		}
	}
}