using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Codinsa2015.Views
{

	public class State
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
		/// <summary>
		///  Obtient toutes les données du jeu qui ne vont pas varier lors de son déroulement. A appeler une
		/// fois en PickPhase (pour récup les sorts) et une fois en GamePhase (pour récup les données de la
		/// map)
		/// </summary>
		public GameStaticDataView GetStaticData(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetStaticData();
		}	
		/// <summary>
		///  Lors de la phase de picks, retourne l'action actuellement attendue de la part de ce héros.
		/// </summary>
		public PickAction Picks_NextAction(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).Picks_NextAction();
		}	
		/// <summary>
		///  Lors de la phase de picks, permet à l'IA d'obtenir la liste des ID des spells actifs disponibles.
		/// </summary>
		public List<int> Picks_GetActiveSpells(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).Picks_GetActiveSpells();
		}	
		/// <summary>
		///  Lors de la phase de picks, permet à l'IA d'obtenir la liste des ID des spells passifs
		/// disponibles.
		/// </summary>
		public List<EntityUniquePassives> Picks_GetPassiveSpells(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).Picks_GetPassiveSpells();
		}	
		/// <summary>
		///  Lors de la phase de picks, permet à l'IA de pick un passif donné (si c'est son tour).
		/// </summary>
		public PickResult Picks_PickPassive(EntityUniquePassives passive, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).Picks_PickPassive(passive);
		}	
		/// <summary>
		///  Lors de la phase de picks, permet à l'IA de pick un spell actif dont l'id est donné (si c'est son
		/// tour).
		/// </summary>
		public PickResult Picks_PickActive(int spell, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).Picks_PickActive(spell);
		}	
		/// <summary>
		///  Achète et équipe un objet d'id donné au shop. Les ids peuvent être obtenus via
		/// ShopGetWeapons(),ShopGetArmors(), ShopGetBoots() etc...
		/// </summary>
		public ShopTransactionResult ShopPurchaseItem(int equipId, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).ShopPurchaseItem(equipId);
		}	
		/// <summary>
		///  Vend l'équipement du type passé en paramètre. (vends l'arme si Weapon, l'armure si Armor
		/// etc...)
		/// </summary>
		public ShopTransactionResult ShopSell(EquipmentType equipType, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).ShopSell(equipType);
		}	
		/// <summary>
		///  Effectue une upgrade d'un équipement indiqué en paramètre.
		/// </summary>
		public ShopTransactionResult ShopUpgrade(EquipmentType equipType, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).ShopUpgrade(equipType);
		}	
		/// <summary>
		///  Obtient la liste des modèles d'armes disponibles au shop.
		/// </summary>
		public List<WeaponModelView> ShopGetWeapons(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).ShopGetWeapons();
		}	
		/// <summary>
		///  Obtient la liste des id des modèles d'armures disponibles au shop.
		/// </summary>
		public List<int> ShopGetArmors(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).ShopGetArmors();
		}	
		/// <summary>
		///  Obtient la liste des id des modèles de bottes disponibles au shop.
		/// </summary>
		public List<int> ShopGetBoots(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).ShopGetBoots();
		}	
		/// <summary>
		///  Obtient la liste des id des enchantements disponibles au shop.
		/// </summary>
		public List<int> ShopGetEnchants(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).ShopGetEnchants();
		}	
		/// <summary>
		///  Obtient le nombre de Point d'améliorations du héros.
		/// </summary>
		public float GetMyPA(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMyPA();
		}	
		/// <summary>
		///  Obtient l'id du modèle d'arme équipé par le héros. (-1 si aucun)
		/// </summary>
		public int GetMyWeaponId(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMyWeaponId();
		}	
		/// <summary>
		///  Obtient le niveau du modèle d'arme équipé par le héros. (-1 si aucune arme équipée)
		/// </summary>
		public int GetMyWeaponLevel(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMyWeaponLevel();
		}	
		/// <summary>
		///  Obtient l'id du modèle d'armure équipé par le héros. (-1 si aucun)
		/// </summary>
		public int GetMyArmorId(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMyArmorId();
		}	
		/// <summary>
		///  Obtient le niveau du modèle d'armure équipé par le héros. (-1 si aucune armure équipée)
		/// </summary>
		public int GetMyArmorLevel(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMyArmorLevel();
		}	
		/// <summary>
		///  Obtient l'id du modèle de bottes équipé par le héros. (-1 si aucun)
		/// </summary>
		public int GetMyBootsId(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMyBootsId();
		}	
		/// <summary>
		///  Obtient le niveau du modèle de bottes équipé par le héros. (-1 si aucune paire équipée)
		/// </summary>
		public int GetMyBootsLevel(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMyBootsLevel();
		}	
		/// <summary>
		///  Obtient l'id du modèle d'enchantement d'arme équipé par le héros. (-1 si aucun)
		/// </summary>
		public int GetMyWeaponEnchantId(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMyWeaponEnchantId();
		}	
		/// <summary>
		///  Retourne une vue vers le héros contrôlé par ce contrôleur.
		/// </summary>
		public EntityBaseView GetMyHero(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMyHero();
		}	
		/// <summary>
		///  Retourne la position du héros.
		/// </summary>
		public Vector2 GetMyPosition(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMyPosition();
		}	
		/// <summary>
		///  Déplace le joueur vers la position donnée en utilisant l'A*.
		/// </summary>
		public bool StartMoveTo(Vector2 position, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).StartMoveTo(position);
		}	
		/// <summary>
		///  Indique si le joueur est entrain de se déplacer en utilisant son A*.
		/// </summary>
		public bool IsAutoMoving(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).IsAutoMoving();
		}	
		/// <summary>
		///  Arrête le déplacement automatique (A*) du joueur.
		/// </summary>
		public bool EndMoveTo(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).EndMoveTo();
		}	
		/// <summary>
		///  Obtient une valeur indiquant si votre équipe possède la vision à la position donnée.
		/// </summary>
		public bool HasSightAt(Vector2 position, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).HasSightAt(position);
		}	
		/// <summary>
		///  Obtient une liste des héros morts.
		/// </summary>
		public List<EntityBaseView> GetDeadHeroes(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetDeadHeroes();
		}	
		/// <summary>
		///  Retourne la liste des entités en vue
		/// </summary>
		public List<EntityBaseView> GetEntitiesInSight(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetEntitiesInSight();
		}	
		/// <summary>
		///  Obtient une vue sur l'entité dont l'id est passé en paramètre. (si l'id retourné est -1 : accès
		/// refusé)
		/// </summary>
		public EntityBaseView GetEntityById(int entityId, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetEntityById(entityId);
		}	
		/// <summary>
		///  Utilise l'arme du héros sur l'entité dont l'id est donné.
		/// </summary>
		public SpellUseResult UseMyWeapon(int entityId, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).UseMyWeapon(entityId);
		}	
		/// <summary>
		///  Obtient l'attack range de l'arme du héros au niveau actuel.
		/// </summary>
		public float GetMyAttackRange(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMyAttackRange();
		}	
		/// <summary>
		///  Obtient les points de la trajectoire du héros;
		/// </summary>
		public List<Vector2> GetMyTrajectory(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMyTrajectory();
		}	
		/// <summary>
		///  Déplace le héros selon la direction donnée.
		/// </summary>
		public bool MoveTowards(Vector2 direction, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).MoveTowards(direction);
		}	
		/// <summary>
		///  Obtient les id des spells possédés par le héros.
		/// </summary>
		public List<int> GetMySpells(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMySpells();
		}	
		/// <summary>
		///  Effectue une upgrade du spell d'id donné (0 ou 1).
		/// </summary>
		public SpellUpgradeResult UpgradeMyActiveSpell(int spellId, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).UpgradeMyActiveSpell(spellId);
		}	
		/// <summary>
		///  Obtient le niveau actuel du spell d'id donné (numéro du spell : 0 ou 1). -1 si le spell n'existe
		/// pas.
		/// </summary>
		public int GetMyActiveSpellLevel(int spellId, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMyActiveSpellLevel(spellId);
		}	
		/// <summary>
		///  Obtient le niveau actuel du spell passif. -1 si erreur.
		/// </summary>
		public int GetMyPassiveSpellLevel(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMyPassiveSpellLevel();
		}	
		/// <summary>
		///  Effectue une upgrade du spell passif du héros.
		/// </summary>
		public SpellUpgradeResult UpgradeMyPassiveSpell(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).UpgradeMyPassiveSpell();
		}	
		/// <summary>
		///  Utilise le sort d'id donné. Retourne true si l'action a été effectuée.
		/// </summary>
		public SpellUseResult UseMySpell(int spellId, SpellCastTargetInfoView target, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).UseMySpell(spellId,target);
		}	
		/// <summary>
		///  Obtient une vue sur le spell du héros contrôlé dont l'id est passé en paramètre. (soit 0 soit 1)
		/// </summary>
		public SpellView GetMySpell(int spellId, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMySpell(spellId);
		}	
		/// <summary>
		///  Obtient la phase actuelle du jeu : Pick (=> phase de picks) ou Game (phase de jeu).
		/// </summary>
		public SceneMode GetMode(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMode();
		}	
		/// <summary>
		///  Obtient la description du spell dont l'id est donné en paramètre.
		/// </summary>
		public SpellLevelDescriptionView GetMySpellCurrentLevelDescription(int spellId, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMySpellCurrentLevelDescription(spellId);
		}	
		// Génère le code pour la fonction de traitement des messages.
		public byte[] ProcessRequest(byte[] request, int clientId)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream(request);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
				System.IO.StreamWriter output;
			int functionId = Int32.Parse(input.ReadLine());
			switch(functionId)
			{
			case 0:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				GameStaticDataView retValue0 = GetStaticData(clientId);
				retValue0.Serialize(output);
				output.Close();
				return s.ToArray();
			case 1:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				PickAction retValue1 = Picks_NextAction(clientId);
				output.WriteLine(((int)retValue1).ToString());
				output.Close();
				return s.ToArray();
			case 2:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<int> retValue2 = Picks_GetActiveSpells(clientId);
				output.WriteLine(retValue2.Count.ToString());
				for(int retValue2_it = 0; retValue2_it < retValue2.Count;retValue2_it++) {
					output.WriteLine(((int)retValue2[retValue2_it]).ToString());
				}
				output.Close();
				return s.ToArray();
			case 3:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<EntityUniquePassives> retValue3 = Picks_GetPassiveSpells(clientId);
				output.WriteLine(retValue3.Count.ToString());
				for(int retValue3_it = 0; retValue3_it < retValue3.Count;retValue3_it++) {
					output.WriteLine(((int)retValue3[retValue3_it]).ToString());
				}
				output.Close();
				return s.ToArray();
			case 4:
				EntityUniquePassives arg4_0 = (EntityUniquePassives)Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				PickResult retValue4 = Picks_PickPassive(arg4_0, clientId);
				output.WriteLine(((int)retValue4).ToString());
				output.Close();
				return s.ToArray();
			case 5:
				int arg5_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				PickResult retValue5 = Picks_PickActive(arg5_0, clientId);
				output.WriteLine(((int)retValue5).ToString());
				output.Close();
				return s.ToArray();
			case 6:
				int arg6_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				ShopTransactionResult retValue6 = ShopPurchaseItem(arg6_0, clientId);
				output.WriteLine(((int)retValue6).ToString());
				output.Close();
				return s.ToArray();
			case 7:
				EquipmentType arg7_0 = (EquipmentType)Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				ShopTransactionResult retValue7 = ShopSell(arg7_0, clientId);
				output.WriteLine(((int)retValue7).ToString());
				output.Close();
				return s.ToArray();
			case 8:
				EquipmentType arg8_0 = (EquipmentType)Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				ShopTransactionResult retValue8 = ShopUpgrade(arg8_0, clientId);
				output.WriteLine(((int)retValue8).ToString());
				output.Close();
				return s.ToArray();
			case 9:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<WeaponModelView> retValue9 = ShopGetWeapons(clientId);
				output.WriteLine(retValue9.Count.ToString());
				for(int retValue9_it = 0; retValue9_it < retValue9.Count;retValue9_it++) {
					retValue9[retValue9_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			case 10:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<int> retValue10 = ShopGetArmors(clientId);
				output.WriteLine(retValue10.Count.ToString());
				for(int retValue10_it = 0; retValue10_it < retValue10.Count;retValue10_it++) {
					output.WriteLine(((int)retValue10[retValue10_it]).ToString());
				}
				output.Close();
				return s.ToArray();
			case 11:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<int> retValue11 = ShopGetBoots(clientId);
				output.WriteLine(retValue11.Count.ToString());
				for(int retValue11_it = 0; retValue11_it < retValue11.Count;retValue11_it++) {
					output.WriteLine(((int)retValue11[retValue11_it]).ToString());
				}
				output.Close();
				return s.ToArray();
			case 12:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<int> retValue12 = ShopGetEnchants(clientId);
				output.WriteLine(retValue12.Count.ToString());
				for(int retValue12_it = 0; retValue12_it < retValue12.Count;retValue12_it++) {
					output.WriteLine(((int)retValue12[retValue12_it]).ToString());
				}
				output.Close();
				return s.ToArray();
			case 13:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				float retValue13 = GetMyPA(clientId);
				output.WriteLine(((float)retValue13).ToString());
				output.Close();
				return s.ToArray();
			case 14:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue14 = GetMyWeaponId(clientId);
				output.WriteLine(((int)retValue14).ToString());
				output.Close();
				return s.ToArray();
			case 15:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue15 = GetMyWeaponLevel(clientId);
				output.WriteLine(((int)retValue15).ToString());
				output.Close();
				return s.ToArray();
			case 16:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue16 = GetMyArmorId(clientId);
				output.WriteLine(((int)retValue16).ToString());
				output.Close();
				return s.ToArray();
			case 17:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue17 = GetMyArmorLevel(clientId);
				output.WriteLine(((int)retValue17).ToString());
				output.Close();
				return s.ToArray();
			case 18:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue18 = GetMyBootsId(clientId);
				output.WriteLine(((int)retValue18).ToString());
				output.Close();
				return s.ToArray();
			case 19:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue19 = GetMyBootsLevel(clientId);
				output.WriteLine(((int)retValue19).ToString());
				output.Close();
				return s.ToArray();
			case 20:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue20 = GetMyWeaponEnchantId(clientId);
				output.WriteLine(((int)retValue20).ToString());
				output.Close();
				return s.ToArray();
			case 21:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				EntityBaseView retValue21 = GetMyHero(clientId);
				retValue21.Serialize(output);
				output.Close();
				return s.ToArray();
			case 22:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				Vector2 retValue22 = GetMyPosition(clientId);
				retValue22.Serialize(output);
				output.Close();
				return s.ToArray();
			case 23:
				Vector2 arg23_0 = Vector2.Deserialize(input);
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				bool retValue23 = StartMoveTo(arg23_0, clientId);
				output.WriteLine(retValue23 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 24:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				bool retValue24 = IsAutoMoving(clientId);
				output.WriteLine(retValue24 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 25:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				bool retValue25 = EndMoveTo(clientId);
				output.WriteLine(retValue25 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 26:
				Vector2 arg26_0 = Vector2.Deserialize(input);
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				bool retValue26 = HasSightAt(arg26_0, clientId);
				output.WriteLine(retValue26 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 27:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<EntityBaseView> retValue27 = GetDeadHeroes(clientId);
				output.WriteLine(retValue27.Count.ToString());
				for(int retValue27_it = 0; retValue27_it < retValue27.Count;retValue27_it++) {
					retValue27[retValue27_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			case 28:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<EntityBaseView> retValue28 = GetEntitiesInSight(clientId);
				output.WriteLine(retValue28.Count.ToString());
				for(int retValue28_it = 0; retValue28_it < retValue28.Count;retValue28_it++) {
					retValue28[retValue28_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			case 29:
				int arg29_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				EntityBaseView retValue29 = GetEntityById(arg29_0, clientId);
				retValue29.Serialize(output);
				output.Close();
				return s.ToArray();
			case 30:
				int arg30_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellUseResult retValue30 = UseMyWeapon(arg30_0, clientId);
				output.WriteLine(((int)retValue30).ToString());
				output.Close();
				return s.ToArray();
			case 31:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				float retValue31 = GetMyAttackRange(clientId);
				output.WriteLine(((float)retValue31).ToString());
				output.Close();
				return s.ToArray();
			case 32:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<Vector2> retValue32 = GetMyTrajectory(clientId);
				output.WriteLine(retValue32.Count.ToString());
				for(int retValue32_it = 0; retValue32_it < retValue32.Count;retValue32_it++) {
					retValue32[retValue32_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			case 33:
				Vector2 arg33_0 = Vector2.Deserialize(input);
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				bool retValue33 = MoveTowards(arg33_0, clientId);
				output.WriteLine(retValue33 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 34:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<int> retValue34 = GetMySpells(clientId);
				output.WriteLine(retValue34.Count.ToString());
				for(int retValue34_it = 0; retValue34_it < retValue34.Count;retValue34_it++) {
					output.WriteLine(((int)retValue34[retValue34_it]).ToString());
				}
				output.Close();
				return s.ToArray();
			case 35:
				int arg35_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellUpgradeResult retValue35 = UpgradeMyActiveSpell(arg35_0, clientId);
				output.WriteLine(((int)retValue35).ToString());
				output.Close();
				return s.ToArray();
			case 36:
				int arg36_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue36 = GetMyActiveSpellLevel(arg36_0, clientId);
				output.WriteLine(((int)retValue36).ToString());
				output.Close();
				return s.ToArray();
			case 37:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue37 = GetMyPassiveSpellLevel(clientId);
				output.WriteLine(((int)retValue37).ToString());
				output.Close();
				return s.ToArray();
			case 38:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellUpgradeResult retValue38 = UpgradeMyPassiveSpell(clientId);
				output.WriteLine(((int)retValue38).ToString());
				output.Close();
				return s.ToArray();
			case 39:
				int arg39_0 = Int32.Parse(input.ReadLine());
				SpellCastTargetInfoView arg39_1 = SpellCastTargetInfoView.Deserialize(input);
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellUseResult retValue39 = UseMySpell(arg39_0, arg39_1, clientId);
				output.WriteLine(((int)retValue39).ToString());
				output.Close();
				return s.ToArray();
			case 40:
				int arg40_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellView retValue40 = GetMySpell(arg40_0, clientId);
				retValue40.Serialize(output);
				output.Close();
				return s.ToArray();
			case 41:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SceneMode retValue41 = GetMode(clientId);
				output.WriteLine(((int)retValue41).ToString());
				output.Close();
				return s.ToArray();
			case 42:
				int arg42_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellLevelDescriptionView retValue42 = GetMySpellCurrentLevelDescription(arg42_0, clientId);
				retValue42.Serialize(output);
				output.Close();
				return s.ToArray();
			}
			return new byte[0];
		}
	
		public State() {
		}

		public static State Deserialize(System.IO.StreamReader input) {
			State _obj =  new State();
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
		}

	}
}
