/** 
 * Contient toutes les informations concernant l'état du serveur.
 */
#include "../inc/State.h"
// 		 * @brief Retourne une vue vers le héros.
		 * @param:lol testtest
		 * @param:mdr test
// test
		 * @returns hahaha
	 
EntityBaseView State::GetHero()
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)0) << '\n';
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
	output << ((int)1) << '\n';
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
	output << ((int)2) << '\n';
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
	output << ((int)3) << '\n';
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
	output << ((int)4) << '\n';
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
	output << ((int)5) << '\n';
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
	output << ((int)6) << '\n';
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
	output << ((int)7) << '\n';
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
	output << ((int)8) << '\n';
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
	output << ((int)9) << '\n';
	output.flush();
	TCPHelper::tcpsend(output);
	std::istringstream input;
	TCPHelper::tcpreceive(input);
	int returnValue; input >> returnValue; input.ignore(1000, '\n');
	return (SceneMode)returnValue;
}



//  Obtient la description du spell dont l'id est donné en paramètre.
SpellDescriptionView State::GetSpellCurrentLevelDescription(int spellId)
{
	std::ostringstream output = std::ostringstream(std::ios::out);
	output << ((int)10) << '\n';
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
	output << ((int)11) << '\n';
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
	output << ((int)12) << '\n';
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
	output << ((int)13) << '\n';
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

