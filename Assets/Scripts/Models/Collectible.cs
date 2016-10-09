
[System.Serializable]
public class Collectible {
	public int id;
	public string prefab { set; get; }
	public int value { set; get; }
	public Collectible(int id, int value) {
		this.id = id;
		this.value = value;
	}

	public Collectible Clone()
	{
		return (Collectible) this.MemberwiseClone();
	}

}

public class Coin: Collectible {
	public Coin(int id, int value) : base(id, value){}
}

public class Pizza: Collectible {
	public Pizza(int id, int value) : base(id, value){}
}


public class Kid: Collectible {
	public Kid(int id, int value) : base(id, value){}
}

public class Drug: Collectible {
	public Drug(int id, int value) : base(id, value){}
}
