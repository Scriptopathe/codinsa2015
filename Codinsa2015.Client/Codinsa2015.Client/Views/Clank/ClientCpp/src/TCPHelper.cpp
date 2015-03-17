#include "../inc/TCPHelper.h"

SOCKET TCPHelper::s_sock;
char* TCPHelper::s_smallBuff;
char* TCPHelper::s_bigBuff;
const int TCPHelper::BIGBUF_LEN = 512;

bool TCPHelper::initialize(std::string ipaddr, int port, std::string nickname)
{
	// Création du socket.
	WSADATA wsadata;
	int error = WSAStartup(0x0202, &wsadata);
	if (error)
	{
		std::cout << "Erreur lors de l'initialisation de l'API socket windows : code " << error << std::endl;
		return false;
	}

	// Création du socket
	s_sock = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (s_sock == INVALID_SOCKET)
	{
		std::cout << "Erreur lors de l'initialisation du socket" << std::endl;
		return false;
	}
	// Création de l'addresse.
	SOCKADDR_IN addr;
	addr.sin_family = AF_INET;
	addr.sin_port = htons(port);
	addr.sin_addr.s_addr = inet_addr(ipaddr.c_str());

	if (connect(s_sock, (sockaddr *)&addr, sizeof(addr)) == SOCKET_ERROR)
	{
		std::cout << "Erreur lors de la connexion à au serveur. " << std::endl;
		return false;
	}

	// Allocation des buffers
	s_smallBuff = (char*)malloc(1);
	s_bigBuff = (char*)malloc(BIGBUF_LEN);

	// Changement de la locale par défaut :
	std::locale::global(std::locale("en-US"));

	// Envoie le nickname
	std::ostringstream stream;
	stream << nickname;
	TCPHelper::tcpsend(stream);
}
/**
Envoie le contenu du stream dans un socket TCP.
*/
void TCPHelper::tcpsend(std::ostringstream& stream)
{
	// utf-8 string
	std::string str = stream.rdbuf()->str();

	std::stringstream ss;
	ss << str.size() << '\n';
	std::string sizeStr = ss.str();

	// Envoie la taille des données + '\n'.
	send(s_sock, sizeStr.c_str(), sizeStr.size(), 0);
	// Envoie les données.
	send(s_sock, str.c_str(), str.size(), 0);
}

/**
* Reçoit un paquet de données Clank et le positionne dans le
* stream passé en paramètre.
*/
void TCPHelper::tcpreceive(std::istringstream& stream)
{
	char last = '\n';
	std::vector<unsigned char> dataBytes;

	// Récupération du nombre de données à lire.
	s_smallBuff[0] = 0;
	while (true)
	{
		int bytes = recv(s_sock, s_smallBuff, 1, 0);
		if (s_smallBuff[0] == last)
			break;
		dataBytes.push_back(s_smallBuff[0]);
	}

	// Transforme ce qu'on a récupéré en un int.
	std::string sizeStr = std::string((char*)&dataBytes[0], dataBytes.size());
	int dataLength = atoi(sizeStr.c_str());
	dataBytes.clear();
	int totalBytes = 0;
	while (totalBytes < dataLength)
	{
		int bytes = recv(s_sock, s_bigBuff, min(dataLength - totalBytes, BIGBUF_LEN), 0);
		totalBytes += bytes;
		for (int i = 0; i < bytes; i++)
		{
			dataBytes.push_back(s_bigBuff[i]);
		}
	}

	std::string data = std::string((char*)&dataBytes[0], dataBytes.size());
	stream.str(data);
}

/**
* Libère la mémoire etc...
*/
void TCPHelper::terminate()
{
	closesocket(s_sock);
	WSACleanup();
	free(s_bigBuff);
	free(s_smallBuff);
}