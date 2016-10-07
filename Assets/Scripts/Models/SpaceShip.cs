using System.Collections.Generic;

[System.Serializable]
public class Spaceship {
	public int id { get; set; }
	public int price {set; get;}
	public long hp {set; get;}
	public int minMedals {set; get;}
	public Gun primaryGun {set; get;}
	public string name { get; set; }
	public string description {get;set;}
	public int velocity { get; set;}

	public Spaceship (int id, int price, long hp, int minMedals, Gun primaryGun, string name = "", string description = "") {
		this.id = id;
		this.price = price;
		this.hp = hp;
		this.minMedals = minMedals;
		this.primaryGun = primaryGun;

		if (name.Length == 0)
			this.name = "Spaceship " + this.id;

		if (description.Length == 0)
			this.description = "Some valid description for spaceship " + this.id;
	}
}
