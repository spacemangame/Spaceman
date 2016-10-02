[System.Serializable]
public class Collectable {
	public int id;

	public long value { set; get; }
	public Collectable(int id, long value) {
		this.id = id;
		this.value = value;
	}
}

public class Coin: Collectable {
	public Coin(int id, long value) : base(id, value){}
}

public class Pizza: Collectable {
	public Pizza(int id, long value) : base(id, value){}
}


public class Kid: Collectable {
	public Kid(int id, long value) : base(id, value){}
}

public class Drug: Collectable {
	public Drug(int id, long value) : base(id, value){}
}
