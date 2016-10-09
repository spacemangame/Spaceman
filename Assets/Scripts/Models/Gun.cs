
[System.Serializable]
public class Gun {
	public int id;
	public string name;
	public int hitPoint;
	public int ammo {set; get;}
	public int maxAmmo {set; get;} // the maximum ammo this gun can have.
	public int currentAmmo {set; get;}
	public int price {set; get;}
	public int ammoPrice {set; get;}
	public int minMedal {set; get;}

	public Gun (int id, string name, int hitPoint, int price, int ammoPrice, int minMedal,  int ammo, int maxAmmo = Constant.maxAmmo) {
		this.id = id;
		this.name = name;
		this.hitPoint = hitPoint;
		this.price = price;
		this.ammoPrice = ammoPrice;
		this.minMedal = minMedal;
		this.ammo = ammo;
		this.currentAmmo = ammo;
		this.maxAmmo = maxAmmo;
	}
}