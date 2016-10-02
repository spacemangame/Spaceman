using System.Collections.Generic;

[System.Serializable]
public class Shop{
	public int id {set; get;}	
	public List<Gun> guns = new List<Gun> (); // all the guns. if player has it, just show buy ammo.
	public List<Spaceship> spaceships = new List<Spaceship>();
}
