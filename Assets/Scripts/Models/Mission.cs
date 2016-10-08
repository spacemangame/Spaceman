using System.Collections.Generic;

[System.Serializable]
public class Mission {
	public int id;

	public int maxMedalEarned;

	public Wave wave;
	public int waveCount;
	public int waveWait;
	public int stabilitliy; //TODO 
	public int currentHp;
	public int currentCoins;
	public List<Gun> activeGuns = new List<Gun>();
	public List<Obstacle> obstacles = new List<Obstacle>();
	public List<Collectible> collectibles = new List<Collectible>();
	public Collectible item; // the item that needs to be delivered/picked up
	public int targetItemCount;
	public int pickedItemCount; // number of medals: (pickedItemCount / targetItemCount) * maxMedalEarned
}

public class PizzaPickUpMission: Mission {
	
}

public class PizzaDeliveryMission: Mission {

}

public class KidPickUpMission: Mission {
	
}

public class KidDeliveryMission: Mission {
	
}