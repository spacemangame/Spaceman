[System.Serializable]
public class Obstacle {
	public int id {set; get;}	
	public int hp {set; get;}
	public int currentHp {set; get;}
	public int velocity {set; get;}
	public string prefab { set; get; }

	//percentage: for example if obstacleCount in wave is 10 and waveRatio of Enemy is 10 then we spwan just 1 enemy in each wave 
	public int waveRatio { set; get; }

	public Obstacle(int id, int hp, int velocity = Constant.defaultVelocity) {
		this.id = id;
		this.hp = hp;
		this.currentHp = hp;
		this.velocity = velocity;
	}

	public Obstacle Clone()
	{
		return (Obstacle) this.MemberwiseClone();
	}
}

public class Asteroid : Obstacle {
	public Asteroid(int id, int hp, int velocity = Constant.defaultVelocity) : base (id, hp, velocity){}
}

public class Enemy : Obstacle {
	public long gunHp {set; get;}

	public Enemy(int id, int hp, int gunHp, int velocity = Constant.defaultVelocity) : base(id, hp, velocity) {
		this.gunHp = gunHp;
	}

}


public class Alien : Obstacle {
	public Alien(int id, int hp, int velocity = Constant.defaultVelocity) : base (id, hp, velocity){}
}
