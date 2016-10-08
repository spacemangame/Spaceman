[System.Serializable]
public class Collectible {
	public int id;
	public string prefab { set; get; }
	public long value { set; get; }
	public Collectible(int id, long value) {
		this.id = id;
		this.value = value;
	}
}

public class Coin: Collectible {
	public Coin(int id, long value) : base(id, value){}
}

public class Pizza: Collectible {
	public Pizza(int id, long value) : base(id, value){}
}


public class Kid: Collectible {
	public Kid(int id, long value) : base(id, value){}
}

public class Drug: Collectible {
	public Drug(int id, long value) : base(id, value){}
}
