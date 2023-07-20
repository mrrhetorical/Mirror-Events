using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Calroot.MirrorEvents.Example {
	public class Player : NetworkBehaviour {

		[SyncVar]
		public string playerName;

		[SerializeField]
		private GameObject localCanvas;

		[SerializeField]
		private TMP_InputField nameInputField;

		[SerializeField]
		private Button nameInputButton;
		
		public void Start() {
			// Begin to listen to events on this class instance
			EventManager.RegisterListeners(this);

			if (!isLocalPlayer)
				localCanvas.SetActive(false);
		}

		public override void OnStartLocalPlayer() {
			base.OnStartLocalPlayer();

			nameInputButton.onClick.AddListener(() => {
				Cmd_ChangeName(nameInputField.text);
			});
		}

		public void OnDestroy() {
			// Stop listening to events on this class instance. Can be called even if it isn't required
			EventManager.UnregisterListeners(this);
		}

		[Command]
		public void Cmd_ChangeName(string newName) {
			// Invoke the name change event
			EventManager.Server_InvokeNetworkedEvent(new PlayerNameChangeEvent {
				netId = netId,
				playerName = newName
			});
		}
		
		// Listen to the PlayerJoinedEvent on the CLIENT here
		[NetworkEventHandler, Client]
		public void Client_OnPlayerJoined(PlayerNameChangeEvent e) {
			if (e.netId != netId) {
				Debug.Log($"Client: The player {e.playerName} had their name changed!");
			} else {
				Debug.Log("Client: You changed your name!");
			}
		}

		// Listen to the PlayerJoinEvent on the SERVER here.
		[NetworkEventHandler, Server]
		public void Server_OnPlayerNameChange(PlayerNameChangeEvent e) {
			if (e.netId != netId)
				return;
			
			playerName = e.playerName;
			Debug.Log($"SERVER: The player {e.playerName} with id {e.id} has a new name!");
		}

		// Listen to the PlayerJoinEvent on the CLIENT and SERVER here
		[NetworkEventHandler]
		public void OnPlayerNameChanged(PlayerNameChangeEvent e) {
			if (e.netId != netId)
				return;
			
			gameObject.name = e.playerName;
		}
	}
}