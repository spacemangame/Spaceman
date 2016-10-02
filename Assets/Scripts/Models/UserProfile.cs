using System.Collections.Generic;

[System.Serializable]
public class UserProfile {
	public int id;
	public Spaceship spaceship {set; get;}
	public long coins {set; get;}
	public long medals {set; get;}
	public int clues {set; get;}
	public List<Gun> guns = new List<Gun>();
}
