
public class Wave {
	public int obstacleCount {set; get;}
	public int collectibleCount { set; get; }
	public int itemCount {set; get;}
	public int spawnWait { set; get; }

	Wave(int obstacleCount, int collectibleCount, int itemCount, int spawnWait) {
		this.obstacleCount = obstacleCount;
		this.collectibleCount = collectibleCount;
		this.itemCount = itemCount;
		this.spawnWait = spawnWait;
	}
}
