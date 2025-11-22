using System.Collections.Generic;

public class AchievementContainer
{
    public AchievementContainer()
    {
        Achievements.Add(new Achievement("Builder", "Build 10 structures.", () => StructuresBuilt(10.0f)));
        Achievements.Add(new Achievement("Commander", "Execute 10 orders.", () => OrdersCompleted(10.0f)));
        Achievements.Add(new Achievement("Destroyer", "Destroy 20 targets.", () => TargetsDestroyed(10.0f)));
        Achievements.Add(new Achievement("Driver", "Drive 30 meters.", () => DistanceDriven(10.0f)));
        Achievements.Add(new Achievement("Producer", "Produce 10 resources.", () => ResourcesProduced(10.0f)));
        Achievements.Add(new Achievement("Researcher", "Spend 10 seconds on researching.", () => TimeResearching(10.0f)));
        Achievements.Add(new Achievement("Soldier", "Fire 20 missiles.", () => MissilesFired(20.0f)));
    }

    public void Update()
    {
        foreach (Achievement i in Achievements)
        {
            if (i.Completed)
            {
                continue;
            }

            i.Check();

            if (i.Completed)
            {
                GameMenu.Instance.Log(string.Format("Achievement {0} unlocked.", i.Name));
            }
        }
    }

    private bool DistanceDriven(float value)
    {
        return Player.Stats.Get(Stats.DistanceDriven) >= value;
    }

    private bool MissilesFired(float value)
    {
        return Player.Stats.Get(Stats.MissilesFired) >= value;
    }

    private bool OrdersCompleted(float value)
    {
        return Player.Stats.Get(Stats.OrdersCompleted) >= value;
    }

    private bool ResourcesProduced(float value)
    {
        return Player.Stats.Get(Stats.ResourcesProduced) >= value;
    }

    private bool StructuresBuilt(float value)
    {
        return Player.Stats.Get(Stats.ObjectsConstructed) >= value;
    }

    private bool TargetsDestroyed(float value)
    {
        return Player.Stats.Get(Stats.TargetsDestroyed) >= value;
    }
    
    private bool TimeResearching(float value)
    {
        return Player.Stats.Get(Stats.TimeResearching) >= value;
    }

    public Player Player { get; set; }

    public List<Achievement> Achievements = new List<Achievement>();
}
