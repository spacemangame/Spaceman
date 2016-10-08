using System;
using System.Collections.Generic;

public static class DataGenerator
{
	
	// Utitlity function to generate all guns and spaceships supported by the game
	// Should be called only once
	public static void GenerateShop() {

		Shop shop = new Shop ();

		GenerateSpaceships (shop.spaceships);
		GenerateGuns (shop.guns);

		GameController.Instance.shop = shop;
	}

	private static void GenerateGuns(List<Gun> guns) {

		guns.Add (new Gun (1, "Assault Rifle HP 2", 2, 100, 2, 0, 1000));
		guns.Add (new Gun (2, "Assault Rifle HP 8", 8, 400, 4, 10, 20)); 
		guns.Add (new Gun (3, "Assault Rifle HP 16", 16, 750, 6, 20, 20));
		guns.Add (new Gun (4, "Assault Rifle HP 20", 20, 1200, 8,  30, 20));
		guns.Add (new Gun (5, "Assault Rifle HP 30", 30, 2000, 10, 40, 20));

	}

	private static void GenerateSpaceships(List<Spaceship> spaceships) {

		spaceships.Add(new Spaceship(1, 2000, 400, 10, new Gun(100, "Primary Gun", 2, -1, -1, 10, -1, -1)));
		spaceships.Add(new Spaceship(2, 5000, 800, 20, new Gun(200, "Primary Gun", 4, -1, -1, 10, -1, -1)));
		spaceships.Add(new Spaceship(3, 10000, 1500, 30, new Gun(300, "Primary Gun", 10, -1, -1, 10, -1, -1)));

	}

	// Utility functions used to generate class objects
	public static void GenerateObstacle(string obstacleType) {
		throw new NotImplementedException ();
	}


	public static UserProfile PopulateUserProfile() {
		UserProfile userProfile = new UserProfile ();
		userProfile.spaceship = new Spaceship (0, 0, 200, 0, new Gun (0, "Primary Gun", 1, -1, -1, 0, -1, -1));
		userProfile.medals = 0;
		// TODO: Add spaceship gun to mission whenyou start it
		return userProfile;
	}

	public static List<Mission> GenerateMissions() {

		List<Mission> missions = new List<Mission> ();

		UserProfile profile = GameController.Instance.profile;

		int medals = (profile.medals == 0) ? 1 : profile.medals;
		int level = (int)Math.Floor ((Double) (medals / 9));
		Spaceship levelSpaceship = GameController.Instance.shop.spaceships [level];

		int obstacleHP = (int) Math.Round((Double) (medals / Constant.missionMaxMedal));
		int enemyHP = ((int) Math.Round((Double) (medals / Constant.missionMaxMedal))) * 2;
		int enemyGunHP = levelSpaceship.primaryGun.hitPont;



		var kidDeliveryMission = new Mission ();
		kidDeliveryMission.activeGuns.Add (profile.spaceship.primaryGun);
		kidDeliveryMission.currentHp = profile.spaceship.hp;
		kidDeliveryMission.id = 1;
		kidDeliveryMission.obstacles.Add (new Asteroid (1, obstacleHP));
		kidDeliveryMission.obstacles.Add (new Alien (2, enemyHP));

		kidDeliveryMission.collectibles.Add (new Coin (1, 1));
		//kidDeliveryMission.wave = new Wave ();
		//kidDeliveryMission.wave.itemCount = 0;
		//kidDeliveryMission.wave.obstacleCount = 15;

		missions.Add (kidDeliveryMission);

		return missions;
	}

}

