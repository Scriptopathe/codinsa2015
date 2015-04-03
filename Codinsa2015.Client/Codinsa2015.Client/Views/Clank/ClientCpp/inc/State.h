/** 
 * Contient toutes les informations concernant l'état du serveur.
 */
#pragma once
#include "Common.h"
#include "WeaponModelView.h"
#include "PassiveEquipmentModelView.h"
#include "WeaponEnchantModelView.h"
#include "EntityBaseView.h"
#include "Vector2.h"
#include "MapView.h"
#include "SpellCastTargetInfoView.h"
#include "SpellDescriptionView.h"
#include "SpellView.h"


class State
{

public: 
	//  Achète et équipe un objet d'id donné au shop. Les ids peuvent être obtenus via
	// ShopGetWeapons(),ShopGetArmors(), ShopGetBoots() etc...
	ShopTransactionResult ShopPurchaseItem(int equipId);

	//  Achète un consommable d'id donné, et le place dans le slot donné.
	ShopTransactionResult ShopPurchaseConsummable(int consummableId,int slot);

	//  Vend l'équipement du type passé en paramètre. (vends l'arme si Weapon, l'armure si Armor
	// etc...)
	ShopTransactionResult ShopSell(EquipmentType equipType);

	//  Vends un consommable situé dans le slot donné.
	ShopTransactionResult ShopSellConsummable(int slot);

	//  Effectue une upgrade d'un équipement indiqué en paramètre.
	ShopTransactionResult ShopUpgrade(EquipmentType equipType);

	//  Obtient la liste des modèles d'armes disponibles au shop.
	std::vector<WeaponModelView> ShopGetWeapons();

	//  Obtient la liste des modèles d'armures disponibles au shop.
	std::vector<PassiveEquipmentModelView> ShopGetArmors();

	//  Obtient la liste des modèles de bottes disponibles au shop.
	std::vector<PassiveEquipmentModelView> ShopGetBoots();

	//  Obtient la liste des enchantements disponibles au shop.
	std::vector<WeaponEnchantModelView> ShopGetEnchants();

	//  Obtient l'id du modèle d'arme équipé par le héros. (-1 si aucun)
	int GetWeaponId();

	//  Obtient le niveau du modèle d'arme équipé par le héros. (-1 si aucune arme équipée)
	int GetWeaponLevel();

	//  Obtient l'id du modèle d'armure équipé par le héros. (-1 si aucun)
	int GetArmorId();

	//  Obtient le niveau du modèle d'armure équipé par le héros. (-1 si aucune armure équipée)
	int GetArmorLevel();

	//  Obtient l'id du modèle de bottes équipé par le héros. (-1 si aucun)
	int GetBootsId();

	//  Obtient le niveau du modèle de bottes équipé par le héros. (-1 si aucune paire équipée)
	int GetBootsLevel();

	//  Obtient l'id du modèle d'enchantement d'arme équipé par le héros. (-1 si aucun)
	int GetWeaponEnchantId();

	//  Retourne une vue vers le héros contrôlé par ce contrôleur.
	EntityBaseView GetHero();

	//  Retourne la position du héros.
	Vector2 GetPosition();

	//  Retourne les informations concernant la map actuelle
	MapView GetMapView();

	//  Déplace le joueur vers la position donnée en utilisant l'A*.
	bool StartMoveTo(Vector2 position);

	//  Indique si le joueur est entrain de se déplacer en utilisant son A*.
	bool IsAutoMoving();

	//  Arrête le déplacement automatique (A*) du joueur.
	bool EndMoveTo();

	//  Retourne la liste des entités en vue
	std::vector<EntityBaseView> GetEntitiesInSight();

	//  Obtient une vue sur l'entité dont l'id est passé en paramètre. (si l'id retourné est -1 : accès
	// refusé)
	EntityBaseView GetEntityById(int entityId);

	//  Utilise le sort d'id donné. Retourne true si l'action a été effectuée.
	bool UseSpell(int spellId,SpellCastTargetInfoView target);

	//  Obtient le mode actuel de la scène.
	SceneMode GetMode();

	//  Obtient la description du spell dont l'id est donné en paramètre.
	SpellDescriptionView GetSpellCurrentLevelDescription(int spellId);

	//  Obtient une vue sur le spell du héros contrôlé dont l'id est passé en paramètre.
	SpellView GetSpell(int spellId);

	//  Obtient la liste des spells du héros contrôlé.
	std::vector<SpellView> GetSpells();

	//  Obtient les spells possédés par le héros dont l'id est passé en paramètre.
	std::vector<SpellView> GetHeroSpells(int entityId);

	void serialize(std::ostream& output);

	static State deserialize(std::istream& input);
};