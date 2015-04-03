package net.codinsa2015;
import java.lang.*;
import java.util.ArrayList;
import java.io.BufferedReader;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.UnsupportedEncodingException;
import net.codinsa2015.ShopTransactionResult.*;
import net.codinsa2015.EquipmentType.*;
import net.codinsa2015.WeaponModelView.*;
import java.util.ArrayList;
import net.codinsa2015.EntityBaseView.*;
import net.codinsa2015.Vector2.*;
import net.codinsa2015.SpellCastTargetInfoView.*;
import net.codinsa2015.SpellView.*;
import net.codinsa2015.SceneMode.*;
import net.codinsa2015.SpellLevelDescriptionView.*;
import net.codinsa2015.GameStaticDataView.*;


@SuppressWarnings("unused")
public class State
{

	//  Achète et équipe un objet d'id donné au shop. Les ids peuvent être obtenus via
	// ShopGetWeapons(),ShopGetArmors(), ShopGetBoots() etc...
	public ShopTransactionResult ShopPurchaseItem(Integer equipId)
	{
		try {
		System.out.println("[ShopPurchaseItem]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)0).toString() + "\n");
		output.append(((Integer)equipId).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		ShopTransactionResult returnValue = ShopTransactionResult.fromValue(Integer.valueOf(input.readLine()));
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Achète un consommable d'id donné, et le place dans le slot donné.
	public ShopTransactionResult ShopPurchaseConsummable(Integer consummableId,Integer slot)
	{
		try {
		System.out.println("[ShopPurchaseConsummable]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)1).toString() + "\n");
		output.append(((Integer)consummableId).toString() + "\n");
		output.append(((Integer)slot).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		ShopTransactionResult returnValue = ShopTransactionResult.fromValue(Integer.valueOf(input.readLine()));
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Vend l'équipement du type passé en paramètre. (vends l'arme si Weapon, l'armure si Armor
	// etc...)
	public ShopTransactionResult ShopSell(EquipmentType equipType)
	{
		try {
		System.out.println("[ShopSell]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)2).toString() + "\n");
		output.append(((Integer)(equipType.getValue())).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		ShopTransactionResult returnValue = ShopTransactionResult.fromValue(Integer.valueOf(input.readLine()));
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Vends un consommable situé dans le slot donné.
	public ShopTransactionResult ShopSellConsummable(Integer slot)
	{
		try {
		System.out.println("[ShopSellConsummable]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)3).toString() + "\n");
		output.append(((Integer)slot).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		ShopTransactionResult returnValue = ShopTransactionResult.fromValue(Integer.valueOf(input.readLine()));
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Effectue une upgrade d'un équipement indiqué en paramètre.
	public ShopTransactionResult ShopUpgrade(EquipmentType equipType)
	{
		try {
		System.out.println("[ShopUpgrade]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)4).toString() + "\n");
		output.append(((Integer)(equipType.getValue())).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		ShopTransactionResult returnValue = ShopTransactionResult.fromValue(Integer.valueOf(input.readLine()));
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient la liste des modèles d'armes disponibles au shop.
	public ArrayList<WeaponModelView> ShopGetWeapons()
	{
		try {
		System.out.println("[ShopGetWeapons]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)5).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		ArrayList<WeaponModelView> returnValue = new ArrayList<WeaponModelView>();
		int returnValue_count = Integer.valueOf(input.readLine());
		for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
			WeaponModelView returnValue_e = WeaponModelView.deserialize(input);
			returnValue.add((WeaponModelView)returnValue_e);
		}
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient la liste des id des modèles d'armures disponibles au shop.
	public ArrayList<Integer> ShopGetArmors()
	{
		try {
		System.out.println("[ShopGetArmors]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)6).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		ArrayList<Integer> returnValue = new ArrayList<Integer>();
		int returnValue_count = Integer.valueOf(input.readLine());
		for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
			int returnValue_e = Integer.valueOf(input.readLine());
			returnValue.add((Integer)returnValue_e);
		}
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient la liste des id des modèles de bottes disponibles au shop.
	public ArrayList<Integer> ShopGetBoots()
	{
		try {
		System.out.println("[ShopGetBoots]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)7).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		ArrayList<Integer> returnValue = new ArrayList<Integer>();
		int returnValue_count = Integer.valueOf(input.readLine());
		for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
			int returnValue_e = Integer.valueOf(input.readLine());
			returnValue.add((Integer)returnValue_e);
		}
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient la liste des id des enchantements disponibles au shop.
	public ArrayList<Integer> ShopGetEnchants()
	{
		try {
		System.out.println("[ShopGetEnchants]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)8).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		ArrayList<Integer> returnValue = new ArrayList<Integer>();
		int returnValue_count = Integer.valueOf(input.readLine());
		for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
			int returnValue_e = Integer.valueOf(input.readLine());
			returnValue.add((Integer)returnValue_e);
		}
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient l'id du modèle d'arme équipé par le héros. (-1 si aucun)
	public Integer GetMyWeaponId()
	{
		try {
		System.out.println("[GetMyWeaponId]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)9).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		int returnValue = Integer.valueOf(input.readLine());
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient le niveau du modèle d'arme équipé par le héros. (-1 si aucune arme équipée)
	public Integer GetMyWeaponLevel()
	{
		try {
		System.out.println("[GetMyWeaponLevel]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)10).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		int returnValue = Integer.valueOf(input.readLine());
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient l'id du modèle d'armure équipé par le héros. (-1 si aucun)
	public Integer GetMyArmorId()
	{
		try {
		System.out.println("[GetMyArmorId]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)11).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		int returnValue = Integer.valueOf(input.readLine());
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient le niveau du modèle d'armure équipé par le héros. (-1 si aucune armure équipée)
	public Integer GetMyArmorLevel()
	{
		try {
		System.out.println("[GetMyArmorLevel]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)12).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		int returnValue = Integer.valueOf(input.readLine());
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient l'id du modèle de bottes équipé par le héros. (-1 si aucun)
	public Integer GetMyBootsId()
	{
		try {
		System.out.println("[GetMyBootsId]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)13).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		int returnValue = Integer.valueOf(input.readLine());
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient le niveau du modèle de bottes équipé par le héros. (-1 si aucune paire équipée)
	public Integer GetMyBootsLevel()
	{
		try {
		System.out.println("[GetMyBootsLevel]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)14).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		int returnValue = Integer.valueOf(input.readLine());
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient l'id du modèle d'enchantement d'arme équipé par le héros. (-1 si aucun)
	public Integer GetMyWeaponEnchantId()
	{
		try {
		System.out.println("[GetMyWeaponEnchantId]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)15).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		int returnValue = Integer.valueOf(input.readLine());
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Retourne une vue vers le héros contrôlé par ce contrôleur.
	public EntityBaseView GetMyHero()
	{
		try {
		System.out.println("[GetMyHero]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)16).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		EntityBaseView returnValue = EntityBaseView.deserialize(input);
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Retourne la position du héros.
	public Vector2 GetMyPosition()
	{
		try {
		System.out.println("[GetMyPosition]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)17).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		Vector2 returnValue = Vector2.deserialize(input);
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Déplace le joueur vers la position donnée en utilisant l'A*.
	public Boolean StartMoveTo(Vector2 position)
	{
		try {
		System.out.println("[StartMoveTo]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)18).toString() + "\n");
		position.serialize(output);
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		boolean returnValue = Integer.valueOf(input.readLine()) == 0 ? false : true;
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Indique si le joueur est entrain de se déplacer en utilisant son A*.
	public Boolean IsAutoMoving()
	{
		try {
		System.out.println("[IsAutoMoving]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)19).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		boolean returnValue = Integer.valueOf(input.readLine()) == 0 ? false : true;
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Arrête le déplacement automatique (A*) du joueur.
	public Boolean EndMoveTo()
	{
		try {
		System.out.println("[EndMoveTo]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)20).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		boolean returnValue = Integer.valueOf(input.readLine()) == 0 ? false : true;
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Retourne la liste des entités en vue
	public ArrayList<EntityBaseView> GetEntitiesInSight()
	{
		try {
		System.out.println("[GetEntitiesInSight]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)21).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		ArrayList<EntityBaseView> returnValue = new ArrayList<EntityBaseView>();
		int returnValue_count = Integer.valueOf(input.readLine());
		for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
			EntityBaseView returnValue_e = EntityBaseView.deserialize(input);
			returnValue.add((EntityBaseView)returnValue_e);
		}
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient une vue sur l'entité dont l'id est passé en paramètre. (si l'id retourné est -1 : accès
	// refusé)
	public EntityBaseView GetEntityById(Integer entityId)
	{
		try {
		System.out.println("[GetEntityById]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)22).toString() + "\n");
		output.append(((Integer)entityId).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		EntityBaseView returnValue = EntityBaseView.deserialize(input);
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient les id des spells possédés par le héros.
	public ArrayList<Integer> GetMySpells()
	{
		try {
		System.out.println("[GetMySpells]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)23).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		ArrayList<Integer> returnValue = new ArrayList<Integer>();
		int returnValue_count = Integer.valueOf(input.readLine());
		for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
			int returnValue_e = Integer.valueOf(input.readLine());
			returnValue.add((Integer)returnValue_e);
		}
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Utilise le sort d'id donné. Retourne true si l'action a été effectuée.
	public Boolean UseMySpell(Integer spellId,SpellCastTargetInfoView target)
	{
		try {
		System.out.println("[UseMySpell]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)24).toString() + "\n");
		output.append(((Integer)spellId).toString() + "\n");
		target.serialize(output);
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		boolean returnValue = Integer.valueOf(input.readLine()) == 0 ? false : true;
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient une vue sur le spell du héros contrôlé dont l'id est passé en paramètre.
	public SpellView GetMySpell(Integer spellId)
	{
		try {
		System.out.println("[GetMySpell]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)25).toString() + "\n");
		output.append(((Integer)spellId).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		SpellView returnValue = SpellView.deserialize(input);
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient le mode actuel de la scène.
	public SceneMode GetMode()
	{
		try {
		System.out.println("[GetMode]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)26).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		SceneMode returnValue = SceneMode.fromValue(Integer.valueOf(input.readLine()));
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient la description du spell dont l'id est donné en paramètre.
	public SpellLevelDescriptionView GetSpellCurrentLevelDescription(Integer spellId)
	{
		try {
		System.out.println("[GetSpellCurrentLevelDescription]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)27).toString() + "\n");
		output.append(((Integer)spellId).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		SpellLevelDescriptionView returnValue = SpellLevelDescriptionView.deserialize(input);
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient toutes les données du jeu qui ne vont pas varier lors de son déroulement.
	public GameStaticDataView GetStaticData()
	{
		try {
		System.out.println("[GetStaticData]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)28).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		GameStaticDataView returnValue = GameStaticDataView.deserialize(input);
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	public State() {
	}

	public static State deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		State _obj =  new State();
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
	}

}
