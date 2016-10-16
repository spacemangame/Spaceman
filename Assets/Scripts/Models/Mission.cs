using System.Collections.Generic;

[System.Serializable]
public class Mission {
	public int id;

	public int maxMedalEarned;
	public string type;

	public string missionName;

	public Wave wave;
	public int waveCount;
	public int waveWait;
	public float stabilitliy; //TODO 
	public int currentHp;
	public int currentCoins;
	public int enemyGunHP { get; set; }
	public List<Gun> activeGuns = new List<Gun>();
	public List<Obstacle> obstacles = new List<Obstacle>();
	public List<Collectible> collectibles = new List<Collectible>();
	public Collectible item; // the item that needs to be delivered/picked up
	public int targetItemCount;
	public int pickedItemCount; // number of medals: (pickedItemCount / targetItemCount) * maxMedalEarned
	public string scene;

	public Mission() {
		this.enemyGunHP = 0;
		this.scene = Constant.defaultScene;
	}
}

public class PizzaPickUpMission: Mission {
	public PizzaPickUpMission() {
		this.missionName = "PizzaPickup";
		this.type = Constant.Pickup;
	}
}

public class PizzaDeliveryMission: Mission {
	public PizzaDeliveryMission() {
		this.missionName = "PizzaDelivery";
		this.type = Constant.Transport;
	}
}

public class KidPickUpMission: Mission {
	public KidPickUpMission() {
		this.missionName = "KidPickup";
		this.type = Constant.Pickup;
	}
}

public class KidDeliveryMission: Mission {
	public KidDeliveryMission() {
		this.missionName = "KidDelivery";
		this.type = Constant.Transport;
	}
}

public class DrugDeliveryMission: Mission {
	public DrugDeliveryMission() {
		this.missionName = "DrugDelivery";
		this.type = Constant.Transport;
	}
}

public class DrugPickupMission: Mission {
	public DrugPickupMission() {
		this.missionName = "DrugPickup";
		this.type = Constant.Pickup;
	}
}