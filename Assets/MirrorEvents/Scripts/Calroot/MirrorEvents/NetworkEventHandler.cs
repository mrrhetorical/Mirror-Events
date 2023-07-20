using System;

namespace Calroot.MirrorEvents {
	[AttributeUsage(AttributeTargets.Method)]
	public class NetworkEventHandler : Attribute {}
}