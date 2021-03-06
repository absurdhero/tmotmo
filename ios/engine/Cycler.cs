using System.Collections.Generic;

/// Cycles through sprite frames at a defined rate
public class Cycler
{
	protected float interval;
	protected int cycles;
	protected IList<Sprite> sprites;
	protected float startTime;
	protected int currentCycle = 0;

	public Cycler (float interval, int totalCycles, float startTime)
	{
		this.interval = interval;
		this.cycles = totalCycles;
		this.startTime = startTime;
		sprites = new List<Sprite>();
	}
	
	public Cycler (float interval, int totalCycles) : this(interval, totalCycles, Time.time) {
	}
	
	public Cycler(float interval) : this(interval, 0) {
	}
	
	public void AddSprite(Sprite sprite) {
		sprites.Add(sprite);
	}

	public int Count() {
		return currentCycle;
	}

	public virtual void Update (float currentTime)
	{
		float run_time = (currentTime - startTime);
		if (cycles != 0 && currentCycle >= cycles) {
			return;
		}

		int index = (int)(run_time / interval);
		if (index != currentCycle) {
			currentCycle = index;
			foreach(var sprite in sprites) {
				sprite.DrawNextFrame();
			}
		}
	}
	
	public float Length() {
		return interval * cycles;
	}
	
	public bool Complete() {
		return currentCycle >= cycles;
	}
}
