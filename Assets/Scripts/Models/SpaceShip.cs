using System.Collections.Generic;


public class Spaceship {
	public int price {set; get;}
	public long hp {set; get;}
	public int minMedals {set; get;}
	public Gun primaryGun {set; get;}

	public Spaceship (int price, long hp, int minMedals, Gun primaryGun) {
		this.price = price;
		this.hp = hp;
		this.minMedals = minMedals;
		this.primaryGun = primaryGun;
	}
}
