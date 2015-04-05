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
import net.codinsa2015.GameStaticDataView.*;
import net.codinsa2015.PickAction.*;
import java.util.ArrayList;
import net.codinsa2015.EntityUniquePassives.*;
import net.codinsa2015.PickResult.*;
import net.codinsa2015.ShopTransactionResult.*;
import net.codinsa2015.EquipmentType.*;
import net.codinsa2015.WeaponModelView.*;
import net.codinsa2015.EntityBaseView.*;
import net.codinsa2015.Vector2.*;
import net.codinsa2015.SpellUseResult.*;
import net.codinsa2015.SpellUpgradeResult.*;
import net.codinsa2015.SpellCastTargetInfoView.*;
import net.codinsa2015.SpellView.*;
import net.codinsa2015.SceneMode.*;
import net.codinsa2015.SpellLevelDescriptionView.*;


@SuppressWarnings("unused")
public class State
{

	//  Obtient toutes les données du jeu qui ne vont pas varier lors de son déroulement. A appeler une
	// fois en PickPhase (pour récup les sorts) et une fois en GamePhase (pour récup les données de la
	// map)
	public GameStaticDataView GetStaticData()
	{
		try {
		System.out.println("[GetStaticData]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)0).toString() + "\n");
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

	//  Lors de la phase de picks, retourne l'action actuellement attendue de la part de ce héros.
	public PickAction Picks_NextAction()
	{
		try {
		System.out.println("[Picks_NextAction]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)1).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		PickAction returnValue = PickAction.fromValue(Integer.valueOf(input.readLine()));
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Lors de la phase de picks, permet à l'IA d'obtenir la liste des ID des spells actifs disponibles.
	public ArrayList<Integer> Picks_GetActiveSpells()
	{
		try {
		System.out.println("[Picks_GetActiveSpells]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)2).toString() + "\n");
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

	//  Lors de la phase de picks, permet à l'IA d'obtenir la liste des ID des spells passifs
	// disponibles.
	public ArrayList<EntityUniquePassives> Picks_GetPassiveSpells()
	{
		try {
		System.out.println("[Picks_GetPassiveSpells]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)3).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		ArrayList<EntityUniquePassives> returnValue = new ArrayList<EntityUniquePassives>();
		int returnValue_count = Integer.valueOf(input.readLine());
		for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
			EntityUniquePassives returnValue_e = EntityUniquePassives.fromValue(Integer.valueOf(input.readLine()));
			returnValue.add((EntityUniquePassives)returnValue_e);
		}
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Lors de la phase de picks, permet à l'IA de pick un passif donné (si c'est son tour).
	public PickResult Picks_PickPassive(EntityUniquePassives passive)
	{
		try {
		System.out.println("[Picks_PickPassive]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)4).toString() + "\n");
		output.append(((Integer)(passive.getValue())).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		PickResult returnValue = PickResult.fromValue(Integer.valueOf(input.readLine()));
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Lors de la phase de picks, permet à l'IA de pick un spell actif dont l'id est donné (si c'est son
	// tour).
	public PickResult Picks_PickActive(Integer spell)
	{
		try {
		System.out.println("[Picks_PickActive]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)5).toString() + "\n");
		output.append(((Integer)spell).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		PickResult returnValue = PickResult.fromValue(Integer.valueOf(input.readLine()));
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Achète et équipe un objet d'id donné au shop. Les ids peuvent être obtenus via
	// ShopGetWeapons(),ShopGetArmors(), ShopGetBoots() etc...
	public ShopTransactionResult ShopPurchaseItem(Integer equipId)
	{
		try {
		System.out.println("[ShopPurchaseItem]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)6).toString() + "\n");
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

	//  Vend l'équipement du type passé en paramètre. (vends l'arme si Weapon, l'armure si Armor
	// etc...)
	public ShopTransactionResult ShopSell(EquipmentType equipType)
	{
		try {
		System.out.println("[ShopSell]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)7).toString() + "\n");
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

	//  Effectue une upgrade d'un équipement indiqué en paramètre.
	public ShopTransactionResult ShopUpgrade(EquipmentType equipType)
	{
		try {
		System.out.println("[ShopUpgrade]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)8).toString() + "\n");
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
		output.append(((Integer)9).toString() + "\n");
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
		output.append(((Integer)10).toString() + "\n");
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
		output.append(((Integer)11).toString() + "\n");
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
		output.append(((Integer)12).toString() + "\n");
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

	//  Obtient le nombre de Point d'améliorations du héros.
	public Float GetMyPA()
	{
		try {
		System.out.println("[GetMyPA]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)13).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		float returnValue = Float.valueOf(input.readLine());
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

	//  Obtient le niveau du modèle d'arme équipé par le héros. (-1 si aucune arme équipée)
	public Integer GetMyWeaponLevel()
	{
		try {
		System.out.println("[GetMyWeaponLevel]");
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

	//  Obtient l'id du modèle d'armure équipé par le héros. (-1 si aucun)
	public Integer GetMyArmorId()
	{
		try {
		System.out.println("[GetMyArmorId]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)16).toString() + "\n");
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
		output.append(((Integer)17).toString() + "\n");
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
		output.append(((Integer)18).toString() + "\n");
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
		output.append(((Integer)19).toString() + "\n");
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
		output.append(((Integer)20).toString() + "\n");
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
		output.append(((Integer)21).toString() + "\n");
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
		output.append(((Integer)22).toString() + "\n");
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
		output.append(((Integer)23).toString() + "\n");
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
		output.append(((Integer)24).toString() + "\n");
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
		output.append(((Integer)25).toString() + "\n");
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

	//  Obtient une valeur indiquant si votre équipe possède la vision à la position donnée.
	public Boolean HasSightAt(Vector2 position)
	{
		try {
		System.out.println("[HasSightAt]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)26).toString() + "\n");
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

	//  Obtient une liste des héros morts.
	public ArrayList<EntityBaseView> GetDeadHeroes()
	{
		try {
		System.out.println("[GetDeadHeroes]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)27).toString() + "\n");
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

	//  Retourne la liste des entités en vue
	public ArrayList<EntityBaseView> GetEntitiesInSight()
	{
		try {
		System.out.println("[GetEntitiesInSight]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)28).toString() + "\n");
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
		output.append(((Integer)29).toString() + "\n");
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

	//  Utilise l'arme du héros sur l'entité dont l'id est donné.
	public SpellUseResult UseMyWeapon(Integer entityId)
	{
		try {
		System.out.println("[UseMyWeapon]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)30).toString() + "\n");
		output.append(((Integer)entityId).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		SpellUseResult returnValue = SpellUseResult.fromValue(Integer.valueOf(input.readLine()));
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient l'attack range de l'arme du héros au niveau actuel.
	public Float GetMyAttackRange()
	{
		try {
		System.out.println("[GetMyAttackRange]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)31).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		float returnValue = Float.valueOf(input.readLine());
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient les points de la trajectoire du héros;
	public ArrayList<Vector2> GetMyTrajectory()
	{
		try {
		System.out.println("[GetMyTrajectory]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)32).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		ArrayList<Vector2> returnValue = new ArrayList<Vector2>();
		int returnValue_count = Integer.valueOf(input.readLine());
		for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
			Vector2 returnValue_e = Vector2.deserialize(input);
			returnValue.add((Vector2)returnValue_e);
		}
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Déplace le héros selon la direction donnée.
	public Boolean MoveTowards(Vector2 direction)
	{
		try {
		System.out.println("[MoveTowards]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)33).toString() + "\n");
		direction.serialize(output);
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

	//  Obtient les id des spells possédés par le héros.
	public ArrayList<Integer> GetMySpells()
	{
		try {
		System.out.println("[GetMySpells]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)34).toString() + "\n");
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

	//  Effectue une upgrade du spell d'id donné (0 ou 1).
	public SpellUpgradeResult UpgradeMyActiveSpell(Integer spellId)
	{
		try {
		System.out.println("[UpgradeMyActiveSpell]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)35).toString() + "\n");
		output.append(((Integer)spellId).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		SpellUpgradeResult returnValue = SpellUpgradeResult.fromValue(Integer.valueOf(input.readLine()));
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient le niveau actuel du spell d'id donné (numéro du spell : 0 ou 1). -1 si le spell n'existe
	// pas.
	public Integer GetMyActiveSpellLevel(Integer spellId)
	{
		try {
		System.out.println("[GetMyActiveSpellLevel]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)36).toString() + "\n");
		output.append(((Integer)spellId).toString() + "\n");
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

	//  Obtient le niveau actuel du spell passif. -1 si erreur.
	public Integer GetMyPassiveSpellLevel()
	{
		try {
		System.out.println("[GetMyPassiveSpellLevel]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)37).toString() + "\n");
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

	//  Effectue une upgrade du spell passif du héros.
	public SpellUpgradeResult UpgradeMyPassiveSpell()
	{
		try {
		System.out.println("[UpgradeMyPassiveSpell]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)38).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		SpellUpgradeResult returnValue = SpellUpgradeResult.fromValue(Integer.valueOf(input.readLine()));
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Utilise le sort d'id donné. Retourne true si l'action a été effectuée.
	public SpellUseResult UseMySpell(Integer spellId,SpellCastTargetInfoView target)
	{
		try {
		System.out.println("[UseMySpell]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)39).toString() + "\n");
		output.append(((Integer)spellId).toString() + "\n");
		target.serialize(output);
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		SpellUseResult returnValue = SpellUseResult.fromValue(Integer.valueOf(input.readLine()));
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	//  Obtient une vue sur le spell du héros contrôlé dont l'id est passé en paramètre. (soit 0 soit 1)
	public SpellView GetMySpell(Integer spellId)
	{
		try {
		System.out.println("[GetMySpell]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)40).toString() + "\n");
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

	//  Obtient la phase actuelle du jeu : Pick (=> phase de picks) ou Game (phase de jeu).
	public SceneMode GetMode()
	{
		try {
		System.out.println("[GetMode]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)41).toString() + "\n");
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
	public SpellLevelDescriptionView GetMySpellCurrentLevelDescription(Integer spellId)
	{
		try {
		System.out.println("[GetMySpellCurrentLevelDescription]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)42).toString() + "\n");
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

	public State() {
	}

	public static State deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		State _obj =  new State();
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
	}

}
