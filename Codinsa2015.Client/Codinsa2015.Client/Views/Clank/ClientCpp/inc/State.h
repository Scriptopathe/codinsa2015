/** 
 * Contient toutes les informations concernant l'état du serveur.
 */
#pragma once
#include "Common.h"
#include "GameStaticDataView.h"
#include "WeaponModelView.h"
#include "EntityBaseView.h"
#include "Vector2.h"
#include "SpellCastTargetInfoView.h"
#include "SpellView.h"
#include "SpellLevelDescriptionView.h"


class State
{

public: 
	//  Obtient toutes les données du jeu qui ne vont pas varier lors de son déroulement. A appeler une
	// fois en PickPhase (pour récup les sorts) et une fois en GamePhase (pour récup les données de la
	// map)
	GameStaticDataView GetStaticData();

	//  Lors de la phase de picks, retourne l'action actuellement attendue de la part de ce héros.
	PickAction Picks_NextAction();

	//  Lors de la phase de picks, permet à l'IA d'obtenir la liste des ID des spells actifs disponibles.
	std::vector<int> Picks_GetActiveSpells();

	//  Lors de la phase de picks, permet à l'IA d'obtenir la liste des ID des spells passifs
	// disponibles.
	std::vector<EntityUniquePassives> Picks_GetPassiveSpells();

	//  Lors de la phase de picks, permet à l'IA de pick un passif donné (si c'est son tour).
	PickResult Picks_PickPassive(EntityUniquePassives passive);

	//  Lors de la phase de picks, permet à l'IA de pick un spell actif dont l'id est donné (si c'est son
	// tour).
	PickResult Picks_PickActive(int spell);

	//  Achète et équipe un objet d'id donné au shop. Les ids peuvent être obtenus via
	// ShopGetWeapons(),ShopGetArmors(), ShopGetBoots() etc...
	ShopTransactionResult ShopPurchaseItem(int equipId);

	//  Vend l'équipement du type passé en paramètre. (vends l'arme si Weapon, l'armure si Armor
	// etc...)
	ShopTransactionResult ShopSell(EquipmentType equipType);

	//  Effectue une upgrade d'un équipement indiqué en paramètre.
	ShopTransactionResult ShopUpgrade(EquipmentType equipType);

	//  Obtient la liste des modèles d'armes disponibles au shop.
	std::vector<WeaponModelView> ShopGetWeapons();

	//  Obtient la liste des id des modèles d'armures disponibles au shop.
	std::vector<int> ShopGetArmors();

	//  Obtient la liste des id des modèles de bottes disponibles au shop.
	std::vector<int> ShopGetBoots();

	//  Obtient la liste des id des enchantements disponibles au shop.
	std::vector<int> ShopGetEnchants();

	//  Obtient le nombre de Point d'améliorations du héros.
	float GetMyPA();

	//  Obtient l'id du modèle d'arme équipé par le héros. (-1 si aucun)
	int GetMyWeaponId();

	//  Obtient le niveau du modèle d'arme équipé par le héros. (-1 si aucune arme équipée)
	int GetMyWeaponLevel();

	//  Obtient l'id du modèle d'armure équipé par le héros. (-1 si aucun)
	int GetMyArmorId();

	//  Obtient le niveau du modèle d'armure équipé par le héros. (-1 si aucune armure équipée)
	int GetMyArmorLevel();

	//  Obtient l'id du modèle de bottes équipé par le héros. (-1 si aucun)
	int GetMyBootsId();

	//  Obtient le niveau du modèle de bottes équipé par le héros. (-1 si aucune paire équipée)
	int GetMyBootsLevel();

	//  Obtient l'id du modèle d'enchantement d'arme équipé par le héros. (-1 si aucun)
	int GetMyWeaponEnchantId();

	//  Retourne une vue vers le héros contrôlé par ce contrôleur.
	EntityBaseView GetMyHero();

	//  Retourne la position du héros.
	Vector2 GetMyPosition();

	//  Déplace le joueur vers la position donnée en utilisant l'A*.
	bool StartMoveTo(Vector2 position);

	//  Indique si le joueur est entrain de se déplacer en utilisant son A*.
	bool IsAutoMoving();

	//  Arrête le déplacement automatique (A*) du joueur.
	bool EndMoveTo();

	//  Obtient une valeur indiquant si votre équipe possède la vision à la position donnée.
	bool HasSightAt(Vector2 position);

	//  Obtient une liste des héros morts.
	std::vector<EntityBaseView> GetDeadHeroes();

	//  Retourne la liste des entités en vue
	std::vector<EntityBaseView> GetEntitiesInSight();

	//  Obtient une vue sur l'entité dont l'id est passé en paramètre. (si l'id retourné est -1 : accès
	// refusé)
	EntityBaseView GetEntityById(int entityId);

	//  Utilise l'arme du héros sur l'entité dont l'id est donné.
	SpellUseResult UseMyWeapon(int entityId);

	//  Obtient l'attack range de l'arme du héros au niveau actuel.
	float GetMyAttackRange();

	//  Obtient les points de la trajectoire du héros;
	std::vector<Vector2> GetMyTrajectory();

	//  Déplace le héros selon la direction donnée.
	bool MoveTowards(Vector2 direction);

	//  Obtient les id des spells possédés par le héros.
	std::vector<int> GetMySpells();

	//  Effectue une upgrade du spell d'id donné (0 ou 1).
	SpellUpgradeResult UpgradeMyActiveSpell(int spellId);

	//  Obtient le niveau actuel du spell d'id donné (numéro du spell : 0 ou 1). -1 si le spell n'existe
	// pas.
	int GetMyActiveSpellLevel(int spellId);

	//  Obtient le niveau actuel du spell passif. -1 si erreur.
	int GetMyPassiveSpellLevel();

	//  Effectue une upgrade du spell passif du héros.
	SpellUpgradeResult UpgradeMyPassiveSpell();

	//  Utilise le sort d'id donné. Retourne true si l'action a été effectuée.
	SpellUseResult UseMySpell(int spellId,SpellCastTargetInfoView target);

	//  Obtient une vue sur le spell du héros contrôlé dont l'id est passé en paramètre. (soit 0 soit 1)
	SpellView GetMySpell(int spellId);

	//  Obtient la phase actuelle du jeu : Pick (=> phase de picks) ou Game (phase de jeu).
	SceneMode GetMode();

	//  Obtient la description du spell dont l'id est donné en paramètre.
	SpellLevelDescriptionView GetMySpellCurrentLevelDescription(int spellId);

	void serialize(std::ostream& output);

	static State deserialize(std::istream& input);
	State();
};