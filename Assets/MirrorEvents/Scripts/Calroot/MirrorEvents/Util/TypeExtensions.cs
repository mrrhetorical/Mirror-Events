using System;

namespace Calroot.MirrorEvents.Util {
	public static class TypeExtensions {
		/// <summary>
		///     Checks if a type extends another type at any point in the inheritance chain.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="otherType"></param>
		public static bool Extends(this Type type, Type otherType) {
			if (type == otherType)
				return true;
			Type baseType = type.BaseType;
			if (baseType == null)
				return false;
			return baseType == otherType || baseType.Extends(otherType);
		}
	}
}