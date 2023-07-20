using Mirror;

namespace Calroot.MirrorEvents.Example {
	public class EventNetworkManager : NetworkManager {
		
		// Register the server to handle network events in OnStartServer()
		public override void OnStartServer() {
			base.OnStartServer();
			
			EventManager.Server_Register();
		}

		// Unregister the server from handling network events in OnStopServer()
		public override void OnStopServer() {
			base.OnStopServer();
			
			EventManager.Server_Unregister();
		}

		// Register the client to handle network events in OnStartClient()
		public override void OnStartClient() {
			base.OnStartClient();
			
			EventManager.Client_Register();
		}

		// Unregister the client from handling network events in OnStopClient()
		public override void OnStopClient() {
			base.OnStopClient();
			
			EventManager.Client_Unregister();
		}
	}
}