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
		///  Achète un consommable d'id donné, et le place dans le slot donné.
		/// </summary>
		public ShopTransactionResult ShopPurchaseConsummable(int consummableId, int slot, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).ShopPurchaseConsummable(consummableId,slot);
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
		///  Vends un consommable situé dans le slot donné.
		/// </summary>
		public ShopTransactionResult ShopSellConsummable(int slot, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).ShopSellConsummable(slot);
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
		///  Obtient les points de la trajectoire du héros;
		/// </summary>
		public List<Vector2> GetMyTrajectory(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMyTrajectory();
		}	
		/// <summary>
		///  Déplace le héros selon la direction donnée. Retourne toujours true.
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
		///  Obtient une vue sur le spell du héros contrôlé dont l'id est passé en paramètre.
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
		/// <summary>
		///  Obtient toutes les données du jeu qui ne vont pas varier lors de son déroulement.
		/// </summary>
		public GameStaticDataView GetStaticData(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetStaticData();
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
				PickAction retValue0 = Picks_NextAction(clientId);
				output.WriteLine(((int)retValue0).ToString());
				output.Close();
				return s.ToArray();
			case 1:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<int> retValue1 = Picks_GetActiveSpells(clientId);
				output.WriteLine(retValue1.Count.ToString());
				for(int retValue1_it = 0; retValue1_it < retValue1.Count;retValue1_it++) {
					output.WriteLine(((int)retValue1[retValue1_it]).ToString());
				}
				output.Close();
				return s.ToArray();
			case 2:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<EntityUniquePassives> retValue2 = Picks_GetPassiveSpells(clientId);
				output.WriteLine(retValue2.Count.ToString());
				for(int retValue2_it = 0; retValue2_it < retValue2.Count;retValue2_it++) {
					output.WriteLine(((int)retValue2[retValue2_it]).ToString());
				}
				output.Close();
				return s.ToArray();
			case 3:
				EntityUniquePassives arg3_0 = (EntityUniquePassives)Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				PickResult retValue3 = Picks_PickPassive(arg3_0, clientId);
				output.WriteLine(((int)retValue3).ToString());
				output.Close();
				return s.ToArray();
			case 4:
				int arg4_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				PickResult retValue4 = Picks_PickActive(arg4_0, clientId);
				output.WriteLine(((int)retValue4).ToString());
				output.Close();
				return s.ToArray();
			case 5:
				int arg5_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				ShopTransactionResult retValue5 = ShopPurchaseItem(arg5_0, clientId);
				output.WriteLine(((int)retValue5).ToString());
				output.Close();
				return s.ToArray();
			case 6:
				int arg6_0 = Int32.Parse(input.ReadLine());
				int arg6_1 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				ShopTransactionResult retValue6 = ShopPurchaseConsummable(arg6_0, arg6_1, clientId);
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
				int arg8_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				ShopTransactionResult retValue8 = ShopSellConsummable(arg8_0, clientId);
				output.WriteLine(((int)retValue8).ToString());
				output.Close();
				return s.ToArray();
			case 9:
				EquipmentType arg9_0 = (EquipmentType)Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				ShopTransactionResult retValue9 = ShopUpgrade(arg9_0, clientId);
				output.WriteLine(((int)retValue9).ToString());
				output.Close();
				return s.ToArray();
			case 10:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<WeaponModelView> retValue10 = ShopGetWeapons(clientId);
				output.WriteLine(retValue10.Count.ToString());
				for(int retValue10_it = 0; retValue10_it < retValue10.Count;retValue10_it++) {
					retValue10[retValue10_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			case 11:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<int> retValue11 = ShopGetArmors(clientId);
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
				List<int> retValue12 = ShopGetBoots(clientId);
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
				List<int> retValue13 = ShopGetEnchants(clientId);
				output.WriteLine(retValue13.Count.ToString());
				for(int retValue13_it = 0; retValue13_it < retValue13.Count;retValue13_it++) {
					output.WriteLine(((int)retValue13[retValue13_it]).ToString());
				}
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
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<EntityBaseView> retValue26 = GetEntitiesInSight(clientId);
				output.WriteLine(retValue26.Count.ToString());
				for(int retValue26_it = 0; retValue26_it < retValue26.Count;retValue26_it++) {
					retValue26[retValue26_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			case 27:
				int arg27_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				EntityBaseView retValue27 = GetEntityById(arg27_0, clientId);
				retValue27.Serialize(output);
				output.Close();
				return s.ToArray();
			case 28:
				int arg28_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellUseResult retValue28 = UseMyWeapon(arg28_0, clientId);
				output.WriteLine(((int)retValue28).ToString());
				output.Close();
				return s.ToArray();
			case 29:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<Vector2> retValue29 = GetMyTrajectory(clientId);
				output.WriteLine(retValue29.Count.ToString());
				for(int retValue29_it = 0; retValue29_it < retValue29.Count;retValue29_it++) {
					retValue29[retValue29_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			case 30:
				Vector2 arg30_0 = Vector2.Deserialize(input);
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				bool retValue30 = MoveTowards(arg30_0, clientId);
				output.WriteLine(retValue30 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 31:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<int> retValue31 = GetMySpells(clientId);
				output.WriteLine(retValue31.Count.ToString());
				for(int retValue31_it = 0; retValue31_it < retValue31.Count;retValue31_it++) {
					output.WriteLine(((int)retValue31[retValue31_it]).ToString());
				}
				output.Close();
				return s.ToArray();
			case 32:
				int arg32_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellUpgradeResult retValue32 = UpgradeMyActiveSpell(arg32_0, clientId);
				output.WriteLine(((int)retValue32).ToString());
				output.Close();
				return s.ToArray();
			case 33:
				int arg33_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue33 = GetMyActiveSpellLevel(arg33_0, clientId);
				output.WriteLine(((int)retValue33).ToString());
				output.Close();
				return s.ToArray();
			case 34:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue34 = GetMyPassiveSpellLevel(clientId);
				output.WriteLine(((int)retValue34).ToString());
				output.Close();
				return s.ToArray();
			case 35:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellUpgradeResult retValue35 = UpgradeMyPassiveSpell(clientId);
				output.WriteLine(((int)retValue35).ToString());
				output.Close();
				return s.ToArray();
			case 36:
				int arg36_0 = Int32.Parse(input.ReadLine());
				SpellCastTargetInfoView arg36_1 = SpellCastTargetInfoView.Deserialize(input);
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellUseResult retValue36 = UseMySpell(arg36_0, arg36_1, clientId);
				output.WriteLine(((int)retValue36).ToString());
				output.Close();
				return s.ToArray();
			case 37:
				int arg37_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellView retValue37 = GetMySpell(arg37_0, clientId);
				retValue37.Serialize(output);
				output.Close();
				return s.ToArray();
			case 38:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SceneMode retValue38 = GetMode(clientId);
				output.WriteLine(((int)retValue38).ToString());
				output.Close();
				return s.ToArray();
			case 39:
				int arg39_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellLevelDescriptionView retValue39 = GetMySpellCurrentLevelDescription(arg39_0, clientId);
				retValue39.Serialize(output);
				output.Close();
				return s.ToArray();
			case 40:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				GameStaticDataView retValue40 = GetStaticData(clientId);
				retValue40.Serialize(output);
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
