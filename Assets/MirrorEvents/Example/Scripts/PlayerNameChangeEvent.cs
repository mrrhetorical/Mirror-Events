using Mirror;

namespace Calroot.MirrorEvents.Example {
	
	/**
	 * This class can do whatever you want so long as it implements NetworkEvent#Write() and NetworkEvent#Read().
	 * Often times it'll have little functionality other than acting as a glorified struct, but there isn't anything
	 * preventing you from using it like a normal class.
	 */
	public class PlayerNameChangeEvent : NetworkEvent {
		
		public uint netId; // netId of the player
		public string playerName; // name of the player
		
		public override void Write(NetworkWriter writer) {
			// Always call the base write function
			base.Write(writer);
			
			// Write all of your data
			writer.WriteUInt(netId);
			writer.WriteString(playerName);
		}
		
		public override void Read(NetworkReader reader) {
			// Always call the base read function
			base.Read(reader);
			
			// Read all of your data in the SAME ORDER you wrote it
			netId = reader.ReadUInt();
			playerName = reader.ReadString();
		}
	}
}