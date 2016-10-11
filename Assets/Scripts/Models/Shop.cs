using System.Collections.Generic;

[System.Serializable]
public class Shop{
	public List<Gun> guns = new List<Gun> (); // all the guns. if player has it, just show buy ammo.
	public List<Spaceship> spaceships = new List<Spaceship>(); // all the spaceships. if player has it, don't show it.
}
