
// Structure.
class Potato
{
	// Variables de la structures
	int thing;
	int private_thing;
	
	// Accesseurs
	public void set_thing();
	public int get_thing();
	
	// Actionneurs
	public void do_something_to_the_thing(); // agit sur private_thing
	
	// Sérialisation.
	public string Serialize(); // génération auto
	public static Potato Deserialize(string filename); // génération auto
}

// Interface avec le serveur.
class Interface
{
	// Variables d'état de l'interface, pouvant être modifiées avant d'être envoyées.
	Potato potato;
	
	// Requête de récupération, sans variable d'état.
	public int getMovePoints()
	{
		return get("http://127.0.0.0.1/move_points");
	}

	// Requête de récupération depuis le serveur
	public void updatePotato()
	{
		return Potato.Deserialize(get("http://127.0.0.1/potato"));
	}
	
	// Changement de l'état interne.
	public void setPotato(Potato p)
	{
		this.potato = p;
	}
	
	// Récupération de l'état.
	public Potato getPotato()
	{
		return this.potato;
	}
	// ENvoie sur le serveur
	public void commitPotato()
	{
		post("http//127.0.0.1/potato", potato.Serialize());
	}
}

class Server
{	
	Potato potato;
	int movePoints;
	
	public void SendMovePoints()
	{
		
	}
}