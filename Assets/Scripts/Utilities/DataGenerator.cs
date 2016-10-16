using System;
using System.Collections.Generic;
using UnityEngine;

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

		guns.Add (new Gun (1, "Assault Rifle HP 2", 2, 100, 2, 0, 2, texture:"Gun2"));
		guns.Add (new Gun (2, "Assault Rifle HP 8", 8, 400, 4, 10, 20, texture:"Gun2")); 
		guns.Add (new Gun (3, "Assault Rifle HP 16", 16, 750, 6, 20, 20, texture:"Gun2"));
		guns.Add (new Gun (4, "Assault Rifle HP 20", 20, 1200, 8,  30, 20, texture:"Gun2"));
		guns.Add (new Gun (5, "Assault Rifle HP 30", 30, 2000, 10, 40, 20, texture:"Gun2"));

	}

	private static void GenerateSpaceships(List<Spaceship> spaceships) {
		spaceships.Add(new Spaceship(0, 300, 50, 0, new Gun (0, "Primary Gun", 1, -1, -1, 0, -1, -1, "Gun1")));
		spaceships.Add(new Spaceship(1, 600, 100, 10, new Gun(100, "Primary Gun", 2, -1, -1, 10, -1, -1, "Gun1")));
		spaceships.Add(new Spaceship(2, 1200, 200, 20, new Gun(200, "Primary Gun", 4, -1, -1, 10, -1, -1, "Gun1")));
		spaceships.Add(new Spaceship(3, 2400, 400, 30, new Gun(300, "Primary Gun", 10, -1, -1, 10, -1, -1, "Gun1")));
	}


	// Utility functions used to generate class objects
	public static void GenerateObstacle(string obstacleType) {
		throw new NotImplementedException ();
	}


	public static UserProfile PopulateUserProfile() {
		UserProfile userProfile = new UserProfile ();
		userProfile.spaceship = GameController.Instance.shop.spaceships[0];
		userProfile.medals = 0;

		userProfile.isSoundEnabled = true;
		userProfile.isAccelerometerEnabled = true;

		// TODO: Add spaceship gun to mission whenyou start it
		return userProfile;
	}

	private static Mission CreateKidDeliveryMission() {
		UserProfile profile = GameController.Instance.profile;

		int medals = (profile.medals == 0) ? 1 : profile.medals;
		int level = (int)Math.Floor (medals / 9.0f);
		Spaceship levelSpaceship = GameController.Instance.shop.spaceships [level];

		int obstacleHP = (int)Math.Ceiling (((double)medals / Constant.missionMaxMedal));
		//obstacleHP	+= 3;

		int enemyHP = ((int) Math.Ceiling((Double) medals / Constant.missionMaxMedal)) * Constant.hpFactor;
		//enemyHP += 5;

		var kidDeliveryMission = new KidDeliveryMission ();
		kidDeliveryMission.activeGuns.Add (profile.spaceship.primaryGun);
		kidDeliveryMission.currentHp = profile.spaceship.hp;
		kidDeliveryMission.id = 1;


		Asteroid obs1 = new Asteroid (1, obstacleHP);
		obs1.prefab = "Asteroid";
		kidDeliveryMission.obstacles.Add (obs1);

		Asteroid obs2 = new Asteroid (2, obstacleHP);
		obs2.prefab = "Asteroid2";
		kidDeliveryMission.obstacles.Add (obs2);

		Asteroid obs3 = new Asteroid (3, obstacleHP);
		obs3.prefab = "Asteroid2";
		kidDeliveryMission.obstacles.Add (obs3);

		Alien alien = new Alien (4, enemyHP);
		alien.isAI = true;
		alien.prefab = "Alien";
		kidDeliveryMission.obstacles.Add (alien);

		Coin coinSphere = new Coin (1, 1);
		coinSphere.prefab = "ColSphere";
		kidDeliveryMission.collectibles.Add (coinSphere);

		Coin coinCube = new Coin (2, 1);
		coinCube.prefab = "Collectibles";
		kidDeliveryMission.collectibles.Add (coinCube);

		kidDeliveryMission.wave = new Wave (Constant.obstacleCount, Constant.collectibleCount,  Constant.waveItemCount, Constant.spawnWait);
		kidDeliveryMission.waveCount = Constant.waveCount;
		kidDeliveryMission.waveWait = Constant.waveWait;
		kidDeliveryMission.stabilitliy = 0.5f;
		kidDeliveryMission.targetItemCount = Constant.targetItemCount;
		kidDeliveryMission.pickedItemCount = Constant.targetItemCount;

		kidDeliveryMission.maxMedalEarned = Constant.maxMedalPerMission;

		int collectibleValue = ((int)((levelSpaceship.price * Constant.hpFactor) / kidDeliveryMission.maxMedalEarned) - 2 ) / Constant.targetItemCount;

		kidDeliveryMission.currentCoins = collectibleValue * kidDeliveryMission.targetItemCount;
		kidDeliveryMission.item = new Kid (1, collectibleValue);

		//TODO:remove this, its just for testing gameover menu in main scene
		//kidDeliveryMission.currentHp = 3;
	
		return kidDeliveryMission;
	}

	private static Mission CreateKidPickupMission() {
		UserProfile profile = GameController.Instance.profile;

		int medals = (profile.medals == 0) ? 1 : profile.medals;
		int level = (int)Math.Floor ((double)medals / Constant.levelMedals);
		Spaceship levelSpaceship = GameController.Instance.shop.spaceships [level];

		int obstacleHP = (int)Math.Ceiling (((double)medals / Constant.missionMaxMedal));
		int enemyHP = ((int) Math.Ceiling((Double) medals / Constant.missionMaxMedal)) * Constant.hpFactor;

		var kidpickupMission = new KidPickUpMission ();
		kidpickupMission.activeGuns.Add (profile.spaceship.primaryGun);
		kidpickupMission.currentHp = profile.spaceship.hp;
		kidpickupMission.id = 2;

		Asteroid obs1 = new Asteroid (1, obstacleHP);
		obs1.prefab = "Asteroid";
		kidpickupMission.obstacles.Add (obs1);

		Asteroid obs2 = new Asteroid (2, obstacleHP);
		obs2.prefab = "Asteroid2";
		kidpickupMission.obstacles.Add (obs2);

		Asteroid obs3 = new Asteroid (3, obstacleHP);
		obs3.prefab = "Asteroid2";
		kidpickupMission.obstacles.Add (obs3);

		Enemy enemy= new Enemy(5, enemyHP, levelSpaceship.primaryGun.hitPoint);
		enemy.prefab = "Enemy Ship";
		kidpickupMission.obstacles.Add (enemy);
		kidpickupMission.enemyGunHP = levelSpaceship.primaryGun.hitPoint;

		kidpickupMission.wave = new Wave (Constant.obstacleCount, Constant.collectibleCount, Constant.waveItemCount, Constant.spawnWait);
		kidpickupMission.waveCount = Constant.waveCount;
		kidpickupMission.waveWait = Constant.waveWait;
		kidpickupMission.stabilitliy = 0.5f;
		kidpickupMission.targetItemCount = Constant.targetItemCount;
		kidpickupMission.pickedItemCount = 0;
		kidpickupMission.maxMedalEarned = Constant.missionMaxMedal;
		int collectibleValue = ((int)((levelSpaceship.price * Constant.hpFactor) / kidpickupMission.maxMedalEarned) - 1 ) / Constant.targetItemCount;

		kidpickupMission.currentCoins = 0;
		kidpickupMission.item = new Kid (1, collectibleValue);

		kidpickupMission.type = Constant.Pickup;

		return kidpickupMission;
	}

	private static Mission CreateDrugMission() {
		UserProfile profile = GameController.Instance.profile;

		int medals = (profile.medals == 0) ? 1 : profile.medals;
		int level = (int)Math.Floor (medals / 9.0f);
		Spaceship levelSpaceship = GameController.Instance.shop.spaceships [level];

		int obstacleHP = (int)Math.Ceiling (((double)medals / Constant.missionMaxMedal));
		//obstacleHP	+= 3;

		int enemyHP = ((int) Math.Ceiling((Double) medals / Constant.missionMaxMedal)) * Constant.hpFactor;
		//enemyHP += 5;

		var drugMision = new DrugPickupMission ();
		drugMision.activeGuns.Add (profile.spaceship.primaryGun);
		drugMision.currentHp = profile.spaceship.hp;
		drugMision.id = 3;

		Coin coinSphere = new Coin (1, 1);
		coinSphere.prefab = "ColSphere";
		drugMision.collectibles.Add (coinSphere);

		drugMision.stabilitliy = 0;

		drugMision.targetItemCount = 15;

		// TODO: Remove this when you think drugs have been added
		drugMision.pickedItemCount = 0;

		drugMision.maxMedalEarned = Constant.maxMedalPerMission;
		int collectibleValue = (int)((levelSpaceship.price * Constant.hpFactor) / Constant.maxMedalPerMission) / Constant.targetItemCount;

		drugMision.currentCoins = 0;
		drugMision.item = new Drug (4, collectibleValue);

		return drugMision;
	}

	public static List<Mission> GenerateMissions() {

		List<Mission> missions = new List<Mission> ();

		missions.Add (CreateKidDeliveryMission());
		missions.Add (CreateKidPickupMission ());
		missions.Add (CreateDrugMission ());

		return missions;
	}

}

