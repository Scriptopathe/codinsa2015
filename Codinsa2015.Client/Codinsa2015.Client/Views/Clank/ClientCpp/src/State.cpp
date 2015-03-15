/** 
 * Contient toutes les informations concernant l'état du serveur.
 */
#include "../inc/State.h"
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

