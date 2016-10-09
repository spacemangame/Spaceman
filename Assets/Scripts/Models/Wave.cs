
public class Wave {
	public int obstacleCount {set; get;}
	public int collectibleCount { set; get; }
	public int itemCount {set; get;}
	public double spawnWait { set; get; }
	public double collectibleSpawnWait { set; get; }

	public Wave(int obstacleCount, int collectibleCount, int itemCount, double spawnWait) {
		this.obstacleCount = obstacleCount;
		this.collectibleCount = collectibleCount;
		this.itemCount = itemCount;
		this.spawnWait = spawnWait;
		this.collectibleSpawnWait = spawnWait * 4;
	}
}
