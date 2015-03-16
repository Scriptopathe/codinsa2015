package net.codinsa2015;

public class TCPHelper {
	
	/** 
	 * Initialise le gestionnaire de connexion TCP.
	 * Cette étape est nécessaire pour garantir le bon fonctionnement
	 * de la classe State.
	 * @param ipaddr addresse ip du destinataire.
	 * @param port port de destination.
	 * @param nickname nom de l'IA qui sera affiché.
	 */
	public static void Initialize(String ipaddr, int port, String nickname)
	{
		
	}
	
	public static byte[] Receive()
	{
		return new byte[5];
	}
	
	
	public static void Send(byte[] data)
	{
		
	}
}
