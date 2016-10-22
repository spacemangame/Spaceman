﻿using System;
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
		guns.Add (new Gun (1, "Assault Rifle HP 2", 2, 100, 2, 0, 40, texture:"Gun2", bolt: Constant.secondaryGunBolt));
		guns.Add (new Gun (2, "Assault Rifle HP 4", 4, 200, 4, 10, 35, texture:"Gun2", bolt: Constant.secondaryGunBolt)); 
		guns.Add (new Gun (3, "Assault Rifle HP 8", 8, 400, 8, 20, 30, texture:"Gun2",  bolt: Constant.secondaryGunBolt));
		guns.Add (new Gun (4, "Assault Rifle HP 16", 16, 800, 16,  30, 25, texture:"Gun2", bolt: Constant.secondaryGunBolt));
		guns.Add (new Gun (5, "Assault Rifle HP 30", 30, 1500, 30, 40, 20, texture:"Gun2", bolt: Constant.secondaryGunBolt));
	}

	private static void GenerateSpaceships(List<Spaceship> spaceships) {
		spaceships.Add(new Spaceship(0, 300, 70, 0, new Gun (0, "Primary Gun", 1, -1, -1, 0, -1, -1, texture:"Gun1")));
		spaceships.Add(new Spaceship(1, 500, 120, 10, new Gun(100, "Primary Gun", 2, -1, -1, 10, -1, -1, texture:"Gun1")));
		spaceships.Add(new Spaceship(2, 1000, 240, 20, new Gun(200, "Primary Gun", 4, -1, -1, 10, -1, -1, texture:"Gun1")));
		spaceships.Add(new Spaceship(3, 1800, 400, 30, new Gun(300, "Primary Gun", 8, -1, -1, 10, -1, -1, texture:"Gun1")));
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


	private static int GetEnemyHP(int medals) {
		return ((int) Math.Ceiling((Double) medals / Constant.missionMaxMedal)) * Constant.hpFactor;
	}

	private static int GetObstacleHP(int medals) {
		return (int)Math.Ceiling (((double)medals / Constant.missionMaxMedal));
	}

	private static int GetMedals(UserProfile profile) {
		return (profile.medals == 0) ? 1 : profile.medals;
	}

	private static void SetMissionParameters( Mission mission) {

		UserProfile profile = GameController.Instance.profile;

		int medals = GetMedals(profile);
		int level = (int)Math.Floor (medals / 9.0f);
		Spaceship levelSpaceship = GameController.Instance.shop.spaceships [level];

		int obstacleHP = GetObstacleHP (medals);
		int enemyHP = GetEnemyHP (medals);

		mission.currentHp = profile.spaceship.hp;

		Asteroid obs1 = new Asteroid (1, obstacleHP);
		obs1.prefab = "Asteroid";
		mission.obstacles.Add (obs1);

		Asteroid obs2 = new Asteroid (2, obstacleHP);
		obs2.prefab = "Asteroid2";
		mission.obstacles.Add (obs2);

		Asteroid obs3 = new Asteroid (3, obstacleHP);
		obs3.prefab = "Asteroid2";
		mission.obstacles.Add (obs3);


		mission.waveCount = Constant.waveCount;
		mission.waveWait = Constant.waveWait;

		mission.maxMedalEarned = Constant.maxMedalPerMission;

		if (mission.type == Constant.Transport) {
			Alien alien = new Alien (4, enemyHP);
			alien.isAI = true;
			alien.prefab = "Alien";
			mission.obstacles.Add (alien);

			mission.stabilitliy = 0.5f;

			Coin coinSphere = new Coin (1, 1);
			coinSphere.prefab = "ColSphere";
			mission.collectibles.Add (coinSphere);

			Coin coinCube = new Coin (2, 1);
			coinCube.prefab = "Collectibles";
			mission.collectibles.Add (coinCube);

			mission.pickedItemCount = Constant.targetItemCount;
			mission.targetItemCount = Constant.targetItemCount;

			int collectibleValue = ((int)((levelSpaceship.price * Constant.hpFactor) / mission.maxMedalEarned - 1)) / Constant.targetItemCount;

			if (mission.missionName.IndexOf ("kid", StringComparison.CurrentCultureIgnoreCase) != -1) {
				mission.item = new Kid (1, collectibleValue);
			} else {
				collectibleValue = collectibleValue - 2;
				mission.item = new Pizza (2, collectibleValue);
			}

			mission.wave = new Wave (Constant.obstacleCount, Constant.collectibleCount,  mission.targetItemCount / mission.waveCount, Constant.spawnWait);
			mission.currentCoins = collectibleValue * mission.targetItemCount;

		} else if (mission.type == Constant.Pickup) {
			Enemy enemy = new Enemy (5, enemyHP, levelSpaceship.primaryGun.hitPoint);
			enemy.prefab = "Enemy Ship";
			mission.obstacles.Add (enemy);
			mission.enemyGunHP = levelSpaceship.primaryGun.hitPoint;

			mission.targetItemCount = Constant.targetItemCount;
			mission.pickedItemCount = 0;
			mission.currentCoins = 0;

			mission.maxMedalEarned = mission.maxMedalEarned - 1;

			int collectibleValue = ((int)((levelSpaceship.price * Constant.hpFactor) / mission.maxMedalEarned)) / Constant.targetItemCount  - 2;

			if (mission.missionName.IndexOf ("kid", StringComparison.CurrentCultureIgnoreCase) != -1) {
				mission.item = new Kid (1, collectibleValue);
			} else {
				collectibleValue = collectibleValue - 2;
				mission.item = new Pizza (2, collectibleValue);
			}

			mission.wave = new Wave (Constant.obstacleCount, Constant.collectibleCount,  mission.targetItemCount / mission.waveCount, Constant.spawnWait);

		} else if (mission.type == Constant.Bonus) {

			Coin coinSphere = new Coin (1, 1);
			coinSphere.prefab = "ColSphere";

			mission.item = coinSphere;

			mission.pickedItemCount = 0;
			mission.targetItemCount = 100;

			mission.wave = new Wave (0, 0 ,  mission.targetItemCount / mission.waveCount, Constant.spawnWait);

			mission.maxMedalEarned = 0;
		}

	}

	private static Mission CreateKidDeliveryMission() {
		var kidDeliveryMission = new KidDeliveryMission ();
		kidDeliveryMission.id = 1;
		SetMissionParameters (kidDeliveryMission);
		return kidDeliveryMission;
	}

	private static Mission CreateKidPickupMission() {
		var kidpickupMission = new KidPickUpMission ();
		kidpickupMission.id = 2;

		SetMissionParameters (kidpickupMission);
		return kidpickupMission;
	}

	private static Mission CreatePizzaDeliveryMission() {
		var pizzaDeliveryMission = new PizzaDeliveryMission ();
		pizzaDeliveryMission.id = 4;
		SetMissionParameters (pizzaDeliveryMission);
		return pizzaDeliveryMission;
	}

	private static Mission CreatePizzaPickupMission() {
		var pizzaPickUpMission = new PizzaPickUpMission ();
		pizzaPickUpMission.id = 5;

		SetMissionParameters (pizzaPickUpMission);
		return pizzaPickUpMission;
	}

	private static Mission GetBonusMission() {
		var kidpickupMission = new KidPickUpMission ();
		kidpickupMission.id = 20;

		SetMissionParameters (kidpickupMission);
		return kidpickupMission;
	}


	private static Mission CreateDrugMission() {
		UserProfile profile = GameController.Instance.profile;

		int medals = (profile.medals == 0) ? 1 : profile.medals;
		int level = (int)Math.Floor (medals / 9.0f);
		Spaceship levelSpaceship = GameController.Instance.shop.spaceships [level];

		var drugMision = new DrugPickupMission ();
		drugMision.scene = "Drug";
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
		missions.Add (CreatePizzaPickupMission ());
		missions.Add (CreatePizzaDeliveryMission ());

		return missions;
	}

}

