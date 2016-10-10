using System;

[System.Serializable]
public static class Constant {
	public const int maxClues = 28;
	public const int defaultVelocity= 1;
	public const int coinValue = 1;
	public const int maxAmmo = 100;
	public const double defaultSpawnWait = 0.5;
	public const int missionMaxMedal = 3;
	public const int levelMedals = 9;
	public const int maxMedalPerMission = 3;
	public const double spawnWait = 0.5;
	public const int waveCount = 10;
	public const int waveWait = 4;
	public const int targetItemCount = 2;
	public const int obstacleCount = 15;
	public const int collectibleCount=10;
	public const int hpFactor = 2;
	public const string Pickup = "Pickup";
	public const string Transport = "Transport";
	public const int waveItemCount = Constant.targetItemCount/ Constant.waveCount;
}


