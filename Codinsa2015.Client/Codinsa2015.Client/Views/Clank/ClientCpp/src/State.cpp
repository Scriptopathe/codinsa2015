/// <summary>
/// Contient toutes les informations concernant l'état du serveur.
/// </summary>
EntityBaseView State::GetHero()
{
	// Send
	FastWriter writer = FastWriter();
	Reader reader = Reader();
	Value obj = Value(arrayValue);
	obj.append(Value(0));
	Value arg = Value(arrayValue);
	obj.append(arg);
	string message = writer.write(obj);
	TCPHelper::send(message);
	// Receive
	string str = TCPHelper::Receive();
	Value jsonResponse;
	reader.parse(str, jsonResponse);
	EntityBaseView response = EntityBaseView::deserialize(jsonResponse);

	return response;
}



Vector2 State::GetPosition()
{
	// Send
	FastWriter writer = FastWriter();
	Reader reader = Reader();
	Value obj = Value(arrayValue);
	obj.append(Value(1));
	Value arg = Value(arrayValue);
	obj.append(arg);
	string message = writer.write(obj);
	TCPHelper::send(message);
	// Receive
	string str = TCPHelper::Receive();
	Value jsonResponse;
	reader.parse(str, jsonResponse);
	Vector2 response = Vector2::deserialize(jsonResponse);

	return response;
}



MapView State::GetMapView()
{
	// Send
	FastWriter writer = FastWriter();
	Reader reader = Reader();
	Value obj = Value(arrayValue);
	obj.append(Value(2));
	Value arg = Value(arrayValue);
	obj.append(arg);
	string message = writer.write(obj);
	TCPHelper::send(message);
	// Receive
	string str = TCPHelper::Receive();
	Value jsonResponse;
	reader.parse(str, jsonResponse);
	MapView response = MapView::deserialize(jsonResponse);

	return response;
}



bool State::StartMoveTo(Vector2 position)
{
	// Send
	FastWriter writer = FastWriter();
	Reader reader = Reader();
	Value obj = Value(arrayValue);
	obj.append(Value(3));
	Value arg = Value(arrayValue);

	// position
	Value position_json = position.serialize();

	arg.append(position_json);
	obj.append(arg);
	string message = writer.write(obj);
	TCPHelper::send(message);
	// Receive
	string str = TCPHelper::Receive();
	Value jsonResponse;
	reader.parse(str, jsonResponse);
	bool response = jsonResponse.asBool();

	return response;
}



bool State::IsAutoMoving()
{
	// Send
	FastWriter writer = FastWriter();
	Reader reader = Reader();
	Value obj = Value(arrayValue);
	obj.append(Value(4));
	Value arg = Value(arrayValue);
	obj.append(arg);
	string message = writer.write(obj);
	TCPHelper::send(message);
	// Receive
	string str = TCPHelper::Receive();
	Value jsonResponse;
	reader.parse(str, jsonResponse);
	bool response = jsonResponse.asBool();

	return response;
}



bool State::EndMoveTo()
{
	// Send
	FastWriter writer = FastWriter();
	Reader reader = Reader();
	Value obj = Value(arrayValue);
	obj.append(Value(5));
	Value arg = Value(arrayValue);
	obj.append(arg);
	string message = writer.write(obj);
	TCPHelper::send(message);
	// Receive
	string str = TCPHelper::Receive();
	Value jsonResponse;
	reader.parse(str, jsonResponse);
	bool response = jsonResponse.asBool();

	return response;
}



vector<EntityBaseView> State::GetEntitiesInSight()
{
	// Send
	FastWriter writer = FastWriter();
	Reader reader = Reader();
	Value obj = Value(arrayValue);
	obj.append(Value(6));
	Value arg = Value(arrayValue);
	obj.append(arg);
	string message = writer.write(obj);
	TCPHelper::send(message);
	// Receive
	string str = TCPHelper::Receive();
	Value jsonResponse;
	reader.parse(str, jsonResponse);
	vector<EntityBaseView> response = vector<EntityBaseView>();
	auto response_iterator = jsonResponse;
	for(auto response_it = response_iterator.begin();response_it != response_iterator.end(); response_it++)
	{
		EntityBaseView response_item = EntityBaseView::deserialize((*response_it));
		response.push_back(response_item);
	}

	return response;
}



EntityBaseView State::GetEntityById(int entityId)
{
	// Send
	FastWriter writer = FastWriter();
	Reader reader = Reader();
	Value obj = Value(arrayValue);
	obj.append(Value(7));
	Value arg = Value(arrayValue);

	// entityId
	Value entityId_json = Value(entityId);

	arg.append(entityId_json);
	obj.append(arg);
	string message = writer.write(obj);
	TCPHelper::send(message);
	// Receive
	string str = TCPHelper::Receive();
	Value jsonResponse;
	reader.parse(str, jsonResponse);
	EntityBaseView response = EntityBaseView::deserialize(jsonResponse);

	return response;
}



bool State::UseSpell(int spellId,SpellCastTargetInfoView target)
{
	// Send
	FastWriter writer = FastWriter();
	Reader reader = Reader();
	Value obj = Value(arrayValue);
	obj.append(Value(8));
	Value arg = Value(arrayValue);

	// spellId
	Value spellId_json = Value(spellId);

	arg.append(spellId_json);

	// target
	Value target_json = target.serialize();

	arg.append(target_json);
	obj.append(arg);
	string message = writer.write(obj);
	TCPHelper::send(message);
	// Receive
	string str = TCPHelper::Receive();
	Value jsonResponse;
	reader.parse(str, jsonResponse);
	bool response = jsonResponse.asBool();

	return response;
}



SceneMode State::GetMode()
{
	// Send
	FastWriter writer = FastWriter();
	Reader reader = Reader();
	Value obj = Value(arrayValue);
	obj.append(Value(9));
	Value arg = Value(arrayValue);
	obj.append(arg);
	string message = writer.write(obj);
	TCPHelper::send(message);
	// Receive
	string str = TCPHelper::Receive();
	Value jsonResponse;
	reader.parse(str, jsonResponse);
	SceneMode response = jsonResponse.asInt();

	return response;
}



SpellDescriptionView State::GetSpellCurrentLevelDescription(int spellId)
{
	// Send
	FastWriter writer = FastWriter();
	Reader reader = Reader();
	Value obj = Value(arrayValue);
	obj.append(Value(10));
	Value arg = Value(arrayValue);

	// spellId
	Value spellId_json = Value(spellId);

	arg.append(spellId_json);
	obj.append(arg);
	string message = writer.write(obj);
	TCPHelper::send(message);
	// Receive
	string str = TCPHelper::Receive();
	Value jsonResponse;
	reader.parse(str, jsonResponse);
	SpellDescriptionView response = SpellDescriptionView::deserialize(jsonResponse);

	return response;
}



SpellView State::GetSpell(int spellId)
{
	// Send
	FastWriter writer = FastWriter();
	Reader reader = Reader();
	Value obj = Value(arrayValue);
	obj.append(Value(11));
	Value arg = Value(arrayValue);

	// spellId
	Value spellId_json = Value(spellId);

	arg.append(spellId_json);
	obj.append(arg);
	string message = writer.write(obj);
	TCPHelper::send(message);
	// Receive
	string str = TCPHelper::Receive();
	Value jsonResponse;
	reader.parse(str, jsonResponse);
	SpellView response = SpellView::deserialize(jsonResponse);

	return response;
}



vector<SpellView> State::GetSpells()
{
	// Send
	FastWriter writer = FastWriter();
	Reader reader = Reader();
	Value obj = Value(arrayValue);
	obj.append(Value(12));
	Value arg = Value(arrayValue);
	obj.append(arg);
	string message = writer.write(obj);
	TCPHelper::send(message);
	// Receive
	string str = TCPHelper::Receive();
	Value jsonResponse;
	reader.parse(str, jsonResponse);
	vector<SpellView> response = vector<SpellView>();
	auto response_iterator = jsonResponse;
	for(auto response_it = response_iterator.begin();response_it != response_iterator.end(); response_it++)
	{
		SpellView response_item = SpellView::deserialize((*response_it));
		response.push_back(response_item);
	}

	return response;
}



vector<SpellView> State::GetHeroSpells(int entityId)
{
	// Send
	FastWriter writer = FastWriter();
	Reader reader = Reader();
	Value obj = Value(arrayValue);
	obj.append(Value(13));
	Value arg = Value(arrayValue);

	// entityId
	Value entityId_json = Value(entityId);

	arg.append(entityId_json);
	obj.append(arg);
	string message = writer.write(obj);
	TCPHelper::send(message);
	// Receive
	string str = TCPHelper::Receive();
	Value jsonResponse;
	reader.parse(str, jsonResponse);
	vector<SpellView> response = vector<SpellView>();
	auto response_iterator = jsonResponse;
	for(auto response_it = response_iterator.begin();response_it != response_iterator.end(); response_it++)
	{
		SpellView response_item = SpellView::deserialize((*response_it));
		response.push_back(response_item);
	}

	return response;
}



Value State::serialize()
{
	Value root0(Json::objectValue);
	return root0;

}

static State State::deserialize(Value& val)
{
	State obj0 = State();
	return obj0;

}


public enum EntityHeroRole
{
	Fighter = 0,
	Mage = 1,
	Tank = 2,
	Max = 2
}

public enum EntityType
{
	Team1 = 2,
	Team2 = 4,
	Teams = 6,
	Structure = 8,
	Tower = 24,
	Team1Tower = 26,
	Team2Tower = 28,
	Inhibitor = 40,
	Team1Inhibitor = 42,
	Team2Inhibitor = 44,
	Spawner = 72,
	Team1Spawner = 74,
	Team2Spawner = 76,
	Idol = 136,
	Team1Idol = 138,
	Team2Idol = 140,
	Monster = 256,
	Creep = 512,
	Team1Creep = 514,
	Team2Creep = 516,
	Boss = 1280,
	Miniboss = 2304,
	AllObjectives = 3576,
	AllTargettableNeutral = 4088,
	Checkpoint = 16384,
	Team1Checkpoint = 16386,
	Team2CheckPoint = 16388,
	Player = 32768,
	Team1Player = 32770,
	Team2Player = 32772,
	AllTeam1 = 33530,
	AllTeam2 = 33532,
	WardPlacement = 65536,
	Ward = 131072,
	Shop = 262144,
	HeroSpawner = 524288,
	AllSaved = 609784
}

public enum EntityTypeRelative
{
	Me = 1,
	Ally = 2,
	Ennemy = 4,
	Structure = 8,
	Tower = 24,
	AllyTower = 26,
	EnnemyTower = 28,
	Inhibitor = 40,
	AllyInhibitor = 42,
	EnnemyInhibitor = 44,
	Spawner = 72,
	AllySpawner = 74,
	EnnemySpawner = 76,
	Idol = 136,
	AllyIdol = 138,
	EnnemyIdol = 140,
	Monster = 256,
	Creep = 512,
	AllyCreep = 514,
	EnnemyCreep = 516,
	Boss = 1280,
	Miniboss = 2304,
	AllObjectives = 3576,
	AllTargettableNeutral = 4088,
	Checkpoint = 16384,
	AllyCheckpoint = 16386,
	EnnemyCheckpoint = 16388,
	Player = 32768,
	AllyPlayer = 32770,
	EnnemyPlayer = 32772,
	AllAlly = 33530,
	AllEnnemy = 33532,
	WardPlacement = 65536,
	Ward = 131072,
	Shop = 262144,
	HeroSpawner = 524288,
	AllSaved = 609784
}

public enum StateAlterationSource
{
	Consumable = 0,
	Armor = 1,
	Weapon = 2,
	Amulet = 3,
	Boots = 4,
	SpellActive = 5,
	SpellPassive = 6
}

public enum StateAlterationType
{
	None = 0,
	Root = 1,
	Silence = 2,
	Interruption = 4,
	Stun = 7,
	CDR = 8,
	MoveSpeed = 16,
	ArmorBuff = 32,
	Regen = 64,
	AttackDamageBuff = 128,
	MaxHP = 256,
	MagicDamageBuff = 512,
	MagicResistBuff = 1024,
	AttackSpeed = 2048,
	Dash = 4096,
	AttackDamage = 8192,
	MagicDamage = 16384,
	TrueDamage = 32768,
	Heal = 65536,
	Stealth = 131072,
	Shield = 524288,
	Sight = 1048576,
	WardSight = 1048576,
	TrueSight = 2097152
}

public enum DashDirectionType
{
	TowardsEntity = 0,
	Direction = 1,
	BackwardsCaster = 2
}

public enum ConsummableType
{
	Empty = 0,
	Ward = 1,
	Unward = 2
}

public enum ConsummableUseResult
{
	Success = 0,
	SuccessAndDestroyed = 1,
	Fail = 2,
	NotUnits = 3
}

public enum ShopTransactionResult
{
	ItemDoesNotExist = 0,
	ItemIsNotAConsummable = 1,
	NoItemToSell = 2,
	NotEnoughMoney = 3,
	NotInShopRange = 4,
	UnavailableItem = 5,
	ProvidedSlotDoesNotExist = 6,
	NoSlotAvailableOnHero = 7,
	EnchantForNoWeapon = 8,
	StackOverflow = 9,
	Success = 10
}

public enum GenericShapeType
{
	Circle = 0,
	Rectangle = 1
}

public enum SpellUseResult
{
	Success = 0,
	InvalidTarget = 1,
	InvalidTargettingType = 2,
	OnCooldown = 3,
	Silenced = 4,
	OutOfRange = 5
}

public enum TargettingType
{
	Targetted = 1,
	Position = 2,
	Direction = 4
}

public enum VisionFlags
{
	None = 0,
	Team1Vision = 1,
	Team2Vision = 2,
	Team1TrueVision = 5,
	Team2TrueVision = 10,
	Team1WardSight = 17,
	Team2WardSight = 18
}

public enum SceneMode
{
	Lobby = 0,
	Pick = 1,
	Game = 2
}

