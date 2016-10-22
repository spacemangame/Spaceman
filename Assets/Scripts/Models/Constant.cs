using System;

[System.Serializable]
public static class Constant {
	public const int maxClues = 28;
	public const int defaultVelocity= 2;
	public const int coinValue = 1;
	public const int maxAmmo = 100;
	public const double defaultSpawnWait = 0.5;
	public const int missionMaxMedal = 3;
	public const int levelMedals = 9;
	public const int maxMedalPerMission = 3;
	public const double spawnWait = 0.5;
	public const int waveCount = 4;
	public const int waveWait = 4;
	public const int targetItemCount = 12;
	public const int obstacleCount = 15;
	public const int collectibleCount = 6;
	public const int hpFactor = 2;
	public const string Pickup = "Pickup";
	public const string Transport = "Transport";
	public const int waveItemCount = Constant.targetItemCount/ Constant.waveCount;
	public const float instabilityFactor = 2.0f;
	public const string gunTexture = "Gun1";
	public const string spaceshipTexture = "Spaceship1";
	public const string defaultScene = "Main";
	public const float reloadTime = 0.25f;
	public const string gunBolt = "Bolt";
	public const string secondaryGunBolt = "Secondary Bolt";
	public const string Bonus = "Bonus";
}


