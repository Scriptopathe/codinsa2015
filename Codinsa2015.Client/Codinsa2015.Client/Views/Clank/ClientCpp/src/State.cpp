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
	int returnValue_asInt; input >> returnValue; input.ignore(1000, '\n');
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
	int returnValue_asInt; input >> returnValue; input.ignore(1000, '\n');
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
	int returnValue_asInt; input >> returnValue; input.ignore(1000, '\n');
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
	int returnValue_asInt; input >> returnValue; input.ignore(1000, '\n');
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
	int returnValue_asInt; input >> returnValue; input.ignore(1000, '\n');
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



//  Obtient la liste des modèles d'armures disponibles au shop.
std::vector<PassiveEquipmentModelView> State::ShopGetArmors()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)6) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	std::vector<PassiveEquipmentModelView> returnValue = std::vector<PassiveEquipmentModelView>();
	int returnValue_count; input >> returnValue_count; input.ignore(1000, '\n');
	for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
		PassiveEquipmentModelView returnValue_e = PassiveEquipmentModelView::deserialize(input);
		returnValue.push_back((PassiveEquipmentModelView)returnValue_e);
	}

	return (std::vector<PassiveEquipmentModelView>)returnValue;
}



//  Obtient la liste des modèles de bottes disponibles au shop.
std::vector<PassiveEquipmentModelView> State::ShopGetBoots()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)7) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	std::vector<PassiveEquipmentModelView> returnValue = std::vector<PassiveEquipmentModelView>();
	int returnValue_count; input >> returnValue_count; input.ignore(1000, '\n');
	for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
		PassiveEquipmentModelView returnValue_e = PassiveEquipmentModelView::deserialize(input);
		returnValue.push_back((PassiveEquipmentModelView)returnValue_e);
	}

	return (std::vector<PassiveEquipmentModelView>)returnValue;
}



//  Obtient la liste des enchantements disponibles au shop.
std::vector<WeaponEnchantModelView> State::ShopGetEnchants()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)8) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	std::vector<WeaponEnchantModelView> returnValue = std::vector<WeaponEnchantModelView>();
	int returnValue_count; input >> returnValue_count; input.ignore(1000, '\n');
	for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
		WeaponEnchantModelView returnValue_e = WeaponEnchantModelView::deserialize(input);
		returnValue.push_back((WeaponEnchantModelView)returnValue_e);
	}

	return (std::vector<WeaponEnchantModelView>)returnValue;
}



//  Obtient l'id du modèle d'arme équipé par le héros. (-1 si aucun)
int State::GetWeaponId()
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
int State::GetWeaponLevel()
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
int State::GetArmorId()
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
int State::GetArmorLevel()
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
int State::GetBootsId()
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
int State::GetBootsLevel()
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
int State::GetWeaponEnchantId()
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
EntityBaseView State::GetHero()
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
Vector2 State::GetPosition()
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



//  Retourne les informations concernant la map actuelle
MapView State::GetMapView()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)18) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	MapView returnValue = MapView::deserialize(input);
	return (MapView)returnValue;
}



//  Déplace le joueur vers la position donnée en utilisant l'A*.
bool State::StartMoveTo(Vector2 position)
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)19) << '\n';
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
	output << ((int)20) << '\n';
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
	output << ((int)21) << '\n';
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
	output << ((int)22) << '\n';
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
	output << ((int)23) << '\n';
	output << ((int)entityId) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	EntityBaseView returnValue = EntityBaseView::deserialize(input);
	return (EntityBaseView)returnValue;
}



//  Utilise le sort d'id donné. Retourne true si l'action a été effectuée.
bool State::UseSpell(int spellId,SpellCastTargetInfoView target)
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



//  Obtient le mode actuel de la scène.
SceneMode State::GetMode()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)25) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	int returnValue_asInt; input >> returnValue; input.ignore(1000, '\n');
	SceneMode returnValue = (SceneMode)returnValue_asInt;
	return (SceneMode)returnValue;
}



//  Obtient la description du spell dont l'id est donné en paramètre.
SpellDescriptionView State::GetSpellCurrentLevelDescription(int spellId)
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)26) << '\n';
	output << ((int)spellId) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	SpellDescriptionView returnValue = SpellDescriptionView::deserialize(input);
	return (SpellDescriptionView)returnValue;
}



//  Obtient une vue sur le spell du héros contrôlé dont l'id est passé en paramètre.
SpellView State::GetSpell(int spellId)
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)27) << '\n';
	output << ((int)spellId) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	SpellView returnValue = SpellView::deserialize(input);
	return (SpellView)returnValue;
}



//  Obtient la liste des spells du héros contrôlé.
std::vector<SpellView> State::GetSpells()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)28) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	std::vector<SpellView> returnValue = std::vector<SpellView>();
	int returnValue_count; input >> returnValue_count; input.ignore(1000, '\n');
	for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
		SpellView returnValue_e = SpellView::deserialize(input);
		returnValue.push_back((SpellView)returnValue_e);
	}

	return (std::vector<SpellView>)returnValue;
}



//  Obtient les spells possédés par le héros dont l'id est passé en paramètre.
std::vector<SpellView> State::GetHeroSpells(int entityId)
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)29) << '\n';
	output << ((int)entityId) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	std::vector<SpellView> returnValue = std::vector<SpellView>();
	int returnValue_count; input >> returnValue_count; input.ignore(1000, '\n');
	for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
		SpellView returnValue_e = SpellView::deserialize(input);
		returnValue.push_back((SpellView)returnValue_e);
	}

	return (std::vector<SpellView>)returnValue;
}



void State::serialize(std::ostream& output) {
}

State State::deserialize(std::istream& input) {
	State _obj = State();
	return _obj;
}

