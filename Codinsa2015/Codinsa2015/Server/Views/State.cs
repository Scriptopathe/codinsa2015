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
		///  Obtient la liste des modèles d'armures disponibles au shop.
		/// </summary>
		public List<PassiveEquipmentModelView> ShopGetArmors(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).ShopGetArmors();
		}	
		/// <summary>
		///  Obtient la liste des modèles de bottes disponibles au shop.
		/// </summary>
		public List<PassiveEquipmentModelView> ShopGetBoots(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).ShopGetBoots();
		}	
		/// <summary>
		///  Obtient la liste des enchantements disponibles au shop.
		/// </summary>
		public List<WeaponEnchantModelView> ShopGetEnchants(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).ShopGetEnchants();
		}	
		/// <summary>
		///  Obtient l'id du modèle d'arme équipé par le héros. (-1 si aucun)
		/// </summary>
		public int GetWeaponId(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetWeaponId();
		}	
		/// <summary>
		///  Obtient le niveau du modèle d'arme équipé par le héros. (-1 si aucune arme équipée)
		/// </summary>
		public int GetWeaponLevel(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetWeaponLevel();
		}	
		/// <summary>
		///  Obtient l'id du modèle d'armure équipé par le héros. (-1 si aucun)
		/// </summary>
		public int GetArmorId(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetArmorId();
		}	
		/// <summary>
		///  Obtient le niveau du modèle d'armure équipé par le héros. (-1 si aucune armure équipée)
		/// </summary>
		public int GetArmorLevel(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetArmorLevel();
		}	
		/// <summary>
		///  Obtient l'id du modèle de bottes équipé par le héros. (-1 si aucun)
		/// </summary>
		public int GetBootsId(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetBootsId();
		}	
		/// <summary>
		///  Obtient le niveau du modèle de bottes équipé par le héros. (-1 si aucune paire équipée)
		/// </summary>
		public int GetBootsLevel(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetBootsLevel();
		}	
		/// <summary>
		///  Obtient l'id du modèle d'enchantement d'arme équipé par le héros. (-1 si aucun)
		/// </summary>
		public int GetWeaponEnchantId(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetWeaponEnchantId();
		}	
		/// <summary>
		///  Retourne une vue vers le héros contrôlé par ce contrôleur.
		/// </summary>
		public EntityBaseView GetHero(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetHero();
		}	
		/// <summary>
		///  Retourne la position du héros.
		/// </summary>
		public Vector2 GetPosition(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetPosition();
		}	
		/// <summary>
		///  Retourne les informations concernant la map actuelle
		/// </summary>
		public MapView GetMapView(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMapView();
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
		///  Utilise le sort d'id donné. Retourne true si l'action a été effectuée.
		/// </summary>
		public bool UseSpell(int spellId, SpellCastTargetInfoView target, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).UseSpell(spellId,target);
		}	
		/// <summary>
		///  Obtient le mode actuel de la scène.
		/// </summary>
		public SceneMode GetMode(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMode();
		}	
		/// <summary>
		///  Obtient la description du spell dont l'id est donné en paramètre.
		/// </summary>
		public SpellDescriptionView GetSpellCurrentLevelDescription(int spellId, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetSpellCurrentLevelDescription(spellId);
		}	
		/// <summary>
		///  Obtient une vue sur le spell du héros contrôlé dont l'id est passé en paramètre.
		/// </summary>
		public SpellView GetSpell(int spellId, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetSpell(spellId);
		}	
		/// <summary>
		///  Obtient la liste des spells du héros contrôlé.
		/// </summary>
		public List<SpellView> GetSpells(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetSpells();
		}	
		/// <summary>
		///  Obtient les spells possédés par le héros dont l'id est passé en paramètre.
		/// </summary>
		public List<SpellView> GetHeroSpells(int entityId, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetHeroSpells(entityId);
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
				int arg0_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				ShopTransactionResult retValue0 = ShopPurchaseItem(arg0_0, clientId);
				output.WriteLine(((int)retValue0).ToString());
				output.Close();
				return s.ToArray();
			case 1:
				int arg1_0 = Int32.Parse(input.ReadLine());
				int arg1_1 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				ShopTransactionResult retValue1 = ShopPurchaseConsummable(arg1_0, arg1_1, clientId);
				output.WriteLine(((int)retValue1).ToString());
				output.Close();
				return s.ToArray();
			case 2:
				EquipmentType arg2_0 = (EquipmentType)Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				ShopTransactionResult retValue2 = ShopSell(arg2_0, clientId);
				output.WriteLine(((int)retValue2).ToString());
				output.Close();
				return s.ToArray();
			case 3:
				int arg3_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				ShopTransactionResult retValue3 = ShopSellConsummable(arg3_0, clientId);
				output.WriteLine(((int)retValue3).ToString());
				output.Close();
				return s.ToArray();
			case 4:
				EquipmentType arg4_0 = (EquipmentType)Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				ShopTransactionResult retValue4 = ShopUpgrade(arg4_0, clientId);
				output.WriteLine(((int)retValue4).ToString());
				output.Close();
				return s.ToArray();
			case 5:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<WeaponModelView> retValue5 = ShopGetWeapons(clientId);
				output.WriteLine(retValue5.Count.ToString());
				for(int retValue5_it = 0; retValue5_it < retValue5.Count;retValue5_it++) {
					retValue5[retValue5_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			case 6:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<PassiveEquipmentModelView> retValue6 = ShopGetArmors(clientId);
				output.WriteLine(retValue6.Count.ToString());
				for(int retValue6_it = 0; retValue6_it < retValue6.Count;retValue6_it++) {
					retValue6[retValue6_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			case 7:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<PassiveEquipmentModelView> retValue7 = ShopGetBoots(clientId);
				output.WriteLine(retValue7.Count.ToString());
				for(int retValue7_it = 0; retValue7_it < retValue7.Count;retValue7_it++) {
					retValue7[retValue7_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			case 8:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<WeaponEnchantModelView> retValue8 = ShopGetEnchants(clientId);
				output.WriteLine(retValue8.Count.ToString());
				for(int retValue8_it = 0; retValue8_it < retValue8.Count;retValue8_it++) {
					retValue8[retValue8_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			case 9:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue9 = GetWeaponId(clientId);
				output.WriteLine(((int)retValue9).ToString());
				output.Close();
				return s.ToArray();
			case 10:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue10 = GetWeaponLevel(clientId);
				output.WriteLine(((int)retValue10).ToString());
				output.Close();
				return s.ToArray();
			case 11:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue11 = GetArmorId(clientId);
				output.WriteLine(((int)retValue11).ToString());
				output.Close();
				return s.ToArray();
			case 12:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue12 = GetArmorLevel(clientId);
				output.WriteLine(((int)retValue12).ToString());
				output.Close();
				return s.ToArray();
			case 13:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue13 = GetBootsId(clientId);
				output.WriteLine(((int)retValue13).ToString());
				output.Close();
				return s.ToArray();
			case 14:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue14 = GetBootsLevel(clientId);
				output.WriteLine(((int)retValue14).ToString());
				output.Close();
				return s.ToArray();
			case 15:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue15 = GetWeaponEnchantId(clientId);
				output.WriteLine(((int)retValue15).ToString());
				output.Close();
				return s.ToArray();
			case 16:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				EntityBaseView retValue16 = GetHero(clientId);
				retValue16.Serialize(output);
				output.Close();
				return s.ToArray();
			case 17:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				Vector2 retValue17 = GetPosition(clientId);
				retValue17.Serialize(output);
				output.Close();
				return s.ToArray();
			case 18:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				MapView retValue18 = GetMapView(clientId);
				retValue18.Serialize(output);
				output.Close();
				return s.ToArray();
			case 19:
				Vector2 arg19_0 = Vector2.Deserialize(input);
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				bool retValue19 = StartMoveTo(arg19_0, clientId);
				output.WriteLine(retValue19 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 20:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				bool retValue20 = IsAutoMoving(clientId);
				output.WriteLine(retValue20 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 21:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				bool retValue21 = EndMoveTo(clientId);
				output.WriteLine(retValue21 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 22:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<EntityBaseView> retValue22 = GetEntitiesInSight(clientId);
				output.WriteLine(retValue22.Count.ToString());
				for(int retValue22_it = 0; retValue22_it < retValue22.Count;retValue22_it++) {
					retValue22[retValue22_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			case 23:
				int arg23_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				EntityBaseView retValue23 = GetEntityById(arg23_0, clientId);
				retValue23.Serialize(output);
				output.Close();
				return s.ToArray();
			case 24:
				int arg24_0 = Int32.Parse(input.ReadLine());
				SpellCastTargetInfoView arg24_1 = SpellCastTargetInfoView.Deserialize(input);
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				bool retValue24 = UseSpell(arg24_0, arg24_1, clientId);
				output.WriteLine(retValue24 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 25:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SceneMode retValue25 = GetMode(clientId);
				output.WriteLine(((int)retValue25).ToString());
				output.Close();
				return s.ToArray();
			case 26:
				int arg26_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellDescriptionView retValue26 = GetSpellCurrentLevelDescription(arg26_0, clientId);
				retValue26.Serialize(output);
				output.Close();
				return s.ToArray();
			case 27:
				int arg27_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellView retValue27 = GetSpell(arg27_0, clientId);
				retValue27.Serialize(output);
				output.Close();
				return s.ToArray();
			case 28:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				List<SpellView> retValue28 = GetSpells(clientId);
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
				List<SpellView> retValue29 = GetHeroSpells(arg29_0, clientId);
				output.WriteLine(retValue29.Count.ToString());
				for(int retValue29_it = 0; retValue29_it < retValue29.Count;retValue29_it++) {
					retValue29[retValue29_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			}
			return new byte[0];
		}
	
		public static State Deserialize(System.IO.StreamReader input) {
			State _obj =  new State();
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
		}

	}
}
