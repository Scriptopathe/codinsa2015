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
import net.codinsa2015.EntityBaseView.*;
import net.codinsa2015.Vector2.*;
import net.codinsa2015.MapView.*;
import java.util.ArrayList;
import net.codinsa2015.SpellCastTargetInfoView.*;
import net.codinsa2015.SceneMode.*;
import net.codinsa2015.SpellDescriptionView.*;
import net.codinsa2015.SpellView.*;


@SuppressWarnings("unused")
public class State
{

	public EntityBaseView GetHero()
	{
		try {
		System.out.println("[GetHero]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)0).toString() + "\n");
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

	public Vector2 GetPosition()
	{
		try {
		System.out.println("[GetPosition]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)1).toString() + "\n");
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

	public MapView GetMapView()
	{
		try {
		System.out.println("[GetMapView]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)2).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		MapView returnValue = MapView.deserialize(input);
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	public Boolean StartMoveTo(Vector2 position)
	{
		try {
		System.out.println("[StartMoveTo]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)3).toString() + "\n");
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

	public Boolean IsAutoMoving()
	{
		try {
		System.out.println("[IsAutoMoving]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)4).toString() + "\n");
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

	public Boolean EndMoveTo()
	{
		try {
		System.out.println("[EndMoveTo]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)5).toString() + "\n");
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

	public ArrayList<EntityBaseView> GetEntitiesInSight()
	{
		try {
		System.out.println("[GetEntitiesInSight]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)6).toString() + "\n");
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

	public EntityBaseView GetEntityById(Integer entityId)
	{
		try {
		System.out.println("[GetEntityById]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)7).toString() + "\n");
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

	public Boolean UseSpell(Integer spellId,SpellCastTargetInfoView target)
	{
		try {
		System.out.println("[UseSpell]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)8).toString() + "\n");
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

	public SceneMode GetMode()
	{
		try {
		System.out.println("[GetMode]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)9).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		int returnValue = Integer.valueOf(input.readLine());
		return SceneMode.fromValue(returnValue);
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	public SpellDescriptionView GetSpellCurrentLevelDescription(Integer spellId)
	{
		try {
		System.out.println("[GetSpellCurrentLevelDescription]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)10).toString() + "\n");
		output.append(((Integer)spellId).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		SpellDescriptionView returnValue = SpellDescriptionView.deserialize(input);
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	public SpellView GetSpell(Integer spellId)
	{
		try {
		System.out.println("[GetSpell]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)11).toString() + "\n");
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

	public ArrayList<SpellView> GetSpells()
	{
		try {
		System.out.println("[GetSpells]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)12).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		ArrayList<SpellView> returnValue = new ArrayList<SpellView>();
		int returnValue_count = Integer.valueOf(input.readLine());
		for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
			SpellView returnValue_e = SpellView.deserialize(input);
			returnValue.add((SpellView)returnValue_e);
		}
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	public ArrayList<SpellView> GetHeroSpells(Integer entityId)
	{
		try {
		System.out.println("[GetHeroSpells]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)13).toString() + "\n");
		output.append(((Integer)entityId).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		ArrayList<SpellView> returnValue = new ArrayList<SpellView>();
		int returnValue_count = Integer.valueOf(input.readLine());
		for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
			SpellView returnValue_e = SpellView.deserialize(input);
			returnValue.add((SpellView)returnValue_e);
		}
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	public static State deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		State _obj =  new State();
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
	}

}
