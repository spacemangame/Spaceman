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
		// TODO: Add spaceship gun to mission whenyou start it
		return userProfile;
	}

	public  static void GenerateMissions() {
		throw new NotImplementedException ();
	}

}

