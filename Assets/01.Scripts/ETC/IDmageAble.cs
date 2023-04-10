using Actors.Bases;

public interface IDmageAble
{
	public float DrainageStat { get; set; }
	public float PercentStat { get; set; }
	public float Half { get; set; }
	public void Damage(float damage, Actor actor)
	{
		
	}

	public void Die()
	{

	}
}
