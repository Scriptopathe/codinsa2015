package net.codinsa2015;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.ArrayList;
import java.util.Locale;

public class TCPHelper {
	static Socket s_sock;
	static byte[] s_smallBuffer;
	static byte[] s_bigBuffer;
	/** 
	 * Initialise le gestionnaire de connexion TCP.
	 * Cette étape est nécessaire pour garantir le bon fonctionnement
	 * de la classe State.
	 * @param ipaddr addresse ip du destinataire.
	 * @param port port de destination.
	 * @param nickname nom de l'IA qui sera affiché.
	 * @throws IOException 
	 * @throws UnknownHostException 
	 */
	public static void Initialize(String ipaddr, int port, String nickname) throws UnknownHostException, IOException
	{
		s_sock = new Socket(ipaddr, port);
		
		// Initialise les buffers
		s_smallBuffer = new byte[1];
		s_bigBuffer = new byte[512];
		
		// Change la locale. (pour obtenir des floats au format US).
		Locale.setDefault(Locale.US);
		
		Send(nickname.getBytes("UTF-8"));
	}
	
	public static byte[] Receive() throws IOException
	{
		byte last = "\n".getBytes("UTF-8")[0];
		
		// Récupère le nb de données à lire.
		ByteArrayOutputStream dataBytes = new ByteArrayOutputStream();
		while(true)
		{
			int bytes = s_sock.getInputStream().read(s_smallBuffer);
			if(s_smallBuffer[0] == last)
				break;
			dataBytes.write(s_smallBuffer);
		}
		
        int dataLength = Integer.valueOf(new String(dataBytes.toByteArray(), "UTF-8"));
        dataBytes.reset();
        int totalBytes = 0;
        while(totalBytes < dataLength)
        {
            int bytes = s_sock.getInputStream().read(s_bigBuffer, 0, Math.min(dataLength - totalBytes, s_bigBuffer.length));
            totalBytes += bytes;
            for(int i = 0; i < bytes; i++)
            {
                dataBytes.write(s_bigBuffer[i]);
            }
        }
        
		return dataBytes.toByteArray();
	}
	
	
	public static void Send(byte[] data) throws UnsupportedEncodingException, IOException
	{
		String lengthString = String.valueOf(data.length) + "\n";
		s_sock.getOutputStream().write(lengthString.getBytes("UTF-8"));
		s_sock.getOutputStream().write(data);;
	}
}
