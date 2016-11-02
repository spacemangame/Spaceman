using System.Collections.Generic;

[System.Serializable]
public class Mission {
	public int id;

	public int maxMedalEarned;
	public int medalEarned;
	public string type;

	public string missionName;

	public Wave wave;
	public int waveCount;
	public int waveWait;
	public float stabilitliy; //TODO 
	public int currentHp;
	public int currentCoins;
	public int enemyGunHP { get; set; }
	public Gun primaryGun {get; set;}
	public Gun secondaryGun {get;set;}
	public List<Obstacle> obstacles = new List<Obstacle>();
	public List<Collectible> collectibles = new List<Collectible>();
	public Collectible item; // the item that needs to be delivered/picked up
	public int targetItemCount;
	public int pickedItemCount; // number of medals: (pickedItemCount / targetItemCount) * maxMedalEarned
	public string scene;

	public string skybox { get; set; }

	public Mission() {
		this.enemyGunHP = 0;
		this.scene = Constant.defaultScene;

		this.skybox = "";
	}
}

public class PizzaPickUpMission: Mission {
	public PizzaPickUpMission() {
		this.missionName = "Pickup Pizza's";
		this.type = Constant.Pickup;
	}
}

public class PizzaDeliveryMission: Mission {
	public PizzaDeliveryMission() {
		this.missionName = "Deliver Pizza's";
		this.type = Constant.Transport;
	}
}

public class KidPickUpMission: Mission {
	public KidPickUpMission() {
		this.missionName = "Pickup Kids";
		this.type = Constant.Pickup;
	}
}

public class KidDeliveryMission: Mission {
	public KidDeliveryMission() {
		this.missionName = "Transport Kids";
		this.type = Constant.Transport;
	}
}

public class DrugDeliveryMission: Mission {
	public DrugDeliveryMission() {
		this.missionName = "Drug Delivery";
		this.type = Constant.Transport;
	}
}

public class DrugPickupMission: Mission {
	public DrugPickupMission() {
		this.missionName = "Drug Pickup";
		this.type = Constant.Pickup;
	}
}

public class BonusMission : Mission {
	public BonusMission() {
		this.missionName = "Bonus Mission";
		this.type = Constant.Bonus;
	}
}