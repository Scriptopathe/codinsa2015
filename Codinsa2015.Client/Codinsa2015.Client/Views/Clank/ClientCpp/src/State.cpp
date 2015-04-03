/** 
 * Contient toutes les informations concernant l'état du serveur.
 */
#include "../inc/State.h"
//  Achète et équipe un objet d'id donné au shop. Les ids peuvent être obtenus via
// ShopGetWeapons(),ShopGetArmors(), ShopGetBoots() etc...
ShopTransactionResult State::ShopPurchaseItem(int equipId)
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)0) << '\n';
	output << ((int)equipId) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	int returnValue_asInt; input >> returnValue_asInt; input.ignore(1000, '\n');
	ShopTransactionResult returnValue = (ShopTransactionResult)returnValue_asInt;
	return (ShopTransactionResult)returnValue;
}



//  Achète un consommable d'id donné, et le place dans le slot donné.
ShopTransactionResult State::ShopPurchaseConsummable(int consummableId,int slot)
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)1) << '\n';
	output << ((int)consummableId) << '\n';
	output << ((int)slot) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	int returnValue_asInt; input >> returnValue_asInt; input.ignore(1000, '\n');
	ShopTransactionResult returnValue = (ShopTransactionResult)returnValue_asInt;
	return (ShopTransactionResult)returnValue;
}



//  Vend l'équipement du type passé en paramètre. (vends l'arme si Weapon, l'armure si Armor
// etc...)
ShopTransactionResult State::ShopSell(EquipmentType equipType)
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)2) << '\n';
	output << ((int)equipType) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	int returnValue_asInt; input >> returnValue_asInt; input.ignore(1000, '\n');
	ShopTransactionResult returnValue = (ShopTransactionResult)returnValue_asInt;
	return (ShopTransactionResult)returnValue;
}



//  Vends un consommable situé dans le slot donné.
ShopTransactionResult State::ShopSellConsummable(int slot)
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)3) << '\n';
	output << ((int)slot) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	int returnValue_asInt; input >> returnValue_asInt; input.ignore(1000, '\n');
	ShopTransactionResult returnValue = (ShopTransactionResult)returnValue_asInt;
	return (ShopTransactionResult)returnValue;
}



//  Effectue une upgrade d'un équipement indiqué en paramètre.
ShopTransactionResult State::ShopUpgrade(EquipmentType equipType)
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)4) << '\n';
	output << ((int)equipType) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	int returnValue_asInt; input >> returnValue_asInt; input.ignore(1000, '\n');
	ShopTransactionResult returnValue = (ShopTransactionResult)returnValue_asInt;
	return (ShopTransactionResult)returnValue;
}



//  Obtient la liste des modèles d'armes disponibles au shop.
std::vector<WeaponModelView> State::ShopGetWeapons()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)5) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	std::vector<WeaponModelView> returnValue = std::vector<WeaponModelView>();
	int returnValue_count; input >> returnValue_count; input.ignore(1000, '\n');
	for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
		WeaponModelView returnValue_e = WeaponModelView::deserialize(input);
		returnValue.push_back((WeaponModelView)returnValue_e);
	}

	return (std::vector<WeaponModelView>)returnValue;
}



//  Obtient la liste des id des modèles d'armures disponibles au shop.
std::vector<int> State::ShopGetArmors()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)6) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	std::vector<int> returnValue = std::vector<int>();
	int returnValue_count; input >> returnValue_count; input.ignore(1000, '\n');
	for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
		int returnValue_e; input >> returnValue_e; input.ignore(1000, '\n');
		returnValue.push_back((int)returnValue_e);
	}

	return (std::vector<int>)returnValue;
}



//  Obtient la liste des id des modèles de bottes disponibles au shop.
std::vector<int> State::ShopGetBoots()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)7) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	std::vector<int> returnValue = std::vector<int>();
	int returnValue_count; input >> returnValue_count; input.ignore(1000, '\n');
	for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
		int returnValue_e; input >> returnValue_e; input.ignore(1000, '\n');
		returnValue.push_back((int)returnValue_e);
	}

	return (std::vector<int>)returnValue;
}



//  Obtient la liste des id des enchantements disponibles au shop.
std::vector<int> State::ShopGetEnchants()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)8) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	std::vector<int> returnValue = std::vector<int>();
	int returnValue_count; input >> returnValue_count; input.ignore(1000, '\n');
	for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
		int returnValue_e; input >> returnValue_e; input.ignore(1000, '\n');
		returnValue.push_back((int)returnValue_e);
	}

	return (std::vector<int>)returnValue;
}



//  Obtient l'id du modèle d'arme équipé par le héros. (-1 si aucun)
int State::GetMyWeaponId()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)9) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	int returnValue; input >> returnValue; input.ignore(1000, '\n');
	return (int)returnValue;
}



//  Obtient le niveau du modèle d'arme équipé par le héros. (-1 si aucune arme équipée)
int State::GetMyWeaponLevel()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)10) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	int returnValue; input >> returnValue; input.ignore(1000, '\n');
	return (int)returnValue;
}



//  Obtient l'id du modèle d'armure équipé par le héros. (-1 si aucun)
int State::GetMyArmorId()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)11) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	int returnValue; input >> returnValue; input.ignore(1000, '\n');
	return (int)returnValue;
}



//  Obtient le niveau du modèle d'armure équipé par le héros. (-1 si aucune armure équipée)
int State::GetMyArmorLevel()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)12) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	int returnValue; input >> returnValue; input.ignore(1000, '\n');
	return (int)returnValue;
}



//  Obtient l'id du modèle de bottes équipé par le héros. (-1 si aucun)
int State::GetMyBootsId()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)13) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	int returnValue; input >> returnValue; input.ignore(1000, '\n');
	return (int)returnValue;
}



//  Obtient le niveau du modèle de bottes équipé par le héros. (-1 si aucune paire équipée)
int State::GetMyBootsLevel()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)14) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	int returnValue; input >> returnValue; input.ignore(1000, '\n');
	return (int)returnValue;
}



//  Obtient l'id du modèle d'enchantement d'arme équipé par le héros. (-1 si aucun)
int State::GetMyWeaponEnchantId()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)15) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	int returnValue; input >> returnValue; input.ignore(1000, '\n');
	return (int)returnValue;
}



//  Retourne une vue vers le héros contrôlé par ce contrôleur.
EntityBaseView State::GetMyHero()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)16) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	EntityBaseView returnValue = EntityBaseView::deserialize(input);
	return (EntityBaseView)returnValue;
}



//  Retourne la position du héros.
Vector2 State::GetMyPosition()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)17) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	Vector2 returnValue = Vector2::deserialize(input);
	return (Vector2)returnValue;
}



//  Déplace le joueur vers la position donnée en utilisant l'A*.
bool State::StartMoveTo(Vector2 position)
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)18) << '\n';
	position.serialize(output);
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	bool returnValue; input >> returnValue; input.ignore(1000, '\n');
	return (bool)returnValue;
}



//  Indique si le joueur est entrain de se déplacer en utilisant son A*.
bool State::IsAutoMoving()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)19) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	bool returnValue; input >> returnValue; input.ignore(1000, '\n');
	return (bool)returnValue;
}



//  Arrête le déplacement automatique (A*) du joueur.
bool State::EndMoveTo()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)20) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	bool returnValue; input >> returnValue; input.ignore(1000, '\n');
	return (bool)returnValue;
}



//  Retourne la liste des entités en vue
std::vector<EntityBaseView> State::GetEntitiesInSight()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)21) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	std::vector<EntityBaseView> returnValue = std::vector<EntityBaseView>();
	int returnValue_count; input >> returnValue_count; input.ignore(1000, '\n');
	for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
		EntityBaseView returnValue_e = EntityBaseView::deserialize(input);
		returnValue.push_back((EntityBaseView)returnValue_e);
	}

	return (std::vector<EntityBaseView>)returnValue;
}



//  Obtient une vue sur l'entité dont l'id est passé en paramètre. (si l'id retourné est -1 : accès
// refusé)
EntityBaseView State::GetEntityById(int entityId)
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)22) << '\n';
	output << ((int)entityId) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	EntityBaseView returnValue = EntityBaseView::deserialize(input);
	return (EntityBaseView)returnValue;
}



//  Obtient les id des spells possédés par le héros.
std::vector<int> State::GetMySpells()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)23) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	std::vector<int> returnValue = std::vector<int>();
	int returnValue_count; input >> returnValue_count; input.ignore(1000, '\n');
	for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
		int returnValue_e; input >> returnValue_e; input.ignore(1000, '\n');
		returnValue.push_back((int)returnValue_e);
	}

	return (std::vector<int>)returnValue;
}



//  Utilise le sort d'id donné. Retourne true si l'action a été effectuée.
bool State::UseMySpell(int spellId,SpellCastTargetInfoView target)
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)24) << '\n';
	output << ((int)spellId) << '\n';
	target.serialize(output);
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	bool returnValue; input >> returnValue; input.ignore(1000, '\n');
	return (bool)returnValue;
}



//  Obtient une vue sur le spell du héros contrôlé dont l'id est passé en paramètre.
SpellView State::GetMySpell(int spellId)
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)25) << '\n';
	output << ((int)spellId) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	SpellView returnValue = SpellView::deserialize(input);
	return (SpellView)returnValue;
}



//  Obtient le mode actuel de la scène.
SceneMode State::GetMode()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)26) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	int returnValue_asInt; input >> returnValue_asInt; input.ignore(1000, '\n');
	SceneMode returnValue = (SceneMode)returnValue_asInt;
	return (SceneMode)returnValue;
}



//  Obtient la description du spell dont l'id est donné en paramètre.
SpellLevelDescriptionView State::GetSpellCurrentLevelDescription(int spellId)
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)27) << '\n';
	output << ((int)spellId) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	SpellLevelDescriptionView returnValue = SpellLevelDescriptionView::deserialize(input);
	return (SpellLevelDescriptionView)returnValue;
}



//  Obtient toutes les données du jeu qui ne vont pas varier lors de son déroulement.
GameStaticDataView State::GetStaticData()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)28) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	GameStaticDataView returnValue = GameStaticDataView::deserialize(input);
	return (GameStaticDataView)returnValue;
}



void State::serialize(std::ostream& output) {
}

State State::deserialize(std::istream& input) {
	State _obj = State();
	return _obj;
}

