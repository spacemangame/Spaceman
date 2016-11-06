using System;

[Serializable]
public class Detonation
{
	public float radius { get; set;}
	public float delay = 0.1f;

	public Detonation (float radius = 3.0f)
	{
		this.radius = radius;
	}

}

