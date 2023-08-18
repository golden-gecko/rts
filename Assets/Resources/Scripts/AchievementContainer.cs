using System.Collections.Generic;
using UnityEngine;

public class AchievementContainer
{
    public AchievementContainer()
    {
        Achievements.Add(new Achievement("Commander", "Execute 10 orders.", () => OrdersCompleted(10.0f)));
        Achievements.Add(new Achievement("Builder", "Build 10 structures.", () => StructuresBuilt(10.0f)));
        Achievements.Add(new Achievement("Driver", "Drive 10 meters.", () => MetersDriven(10.0f)));
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

    private bool OrdersCompleted(float value)
    {
        float sum = 0;

        foreach (MyGameObject myGameObject in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (myGameObject.Player != Player)
            {
                continue;
            }

            sum += myGameObject.Stats.Get(Stats.OrdersCompleted);
        }

        return sum >= value;
    }

    private bool StructuresBuilt(float value)
    {
        float sum = 0;

        foreach (Constructor constructor in GameObject.FindObjectsByType<Constructor>(FindObjectsSortMode.None))
        {
            MyGameObject myGameObject = constructor.GetComponentInParent<MyGameObject>();

            if (myGameObject.Player != Player)
            {
                continue;
            }

            sum += myGameObject.Stats.Get(Stats.ObjectsConstructed);
        }

        return sum >= value;
    }

    private bool MetersDriven(float value)
    {
        float sum = 0;

        foreach (Engine engine in GameObject.FindObjectsByType<Engine>(FindObjectsSortMode.None))
        {
            MyGameObject myGameObject = engine.GetComponentInParent<MyGameObject>();

            if (myGameObject.Player != Player)
            {
                continue;
            }

            sum += myGameObject.Stats.Get(Stats.DistanceDriven);
        }

        return sum >= value;
    }

    private bool ResourcesProduced(float value)
    {
        float sum = 0;

        foreach (Producer producer in GameObject.FindObjectsByType<Producer>(FindObjectsSortMode.None))
        {
            MyGameObject myGameObject = producer.GetComponentInParent<MyGameObject>();

            if (myGameObject.Player != Player)
            {
                continue;
            }

            sum += myGameObject.Stats.Get(Stats.ResourcesProduced);
        }

        return sum >= value;
    }

    private bool TimeResearching(float value)
    {
        float sum = 0;

        foreach (Researcher researcher in GameObject.FindObjectsByType<Researcher>(FindObjectsSortMode.None))
        {
            MyGameObject myGameObject = researcher.GetComponentInParent<MyGameObject>();

            if (myGameObject.Player != Player)
            {
                continue;
            }

            sum += myGameObject.Stats.Get(Stats.TimeResearching);
        }

        return sum >= value;
    }

    private bool MissilesFired(float value)
    {
        float sum = 0;

        foreach (Gun gun in GameObject.FindObjectsByType<Gun>(FindObjectsSortMode.None))
        {
            MyGameObject myGameObject = gun.GetComponentInParent<MyGameObject>();

            if (myGameObject.Player != Player)
            {
                continue;
            }

            sum += myGameObject.Stats.Get(Stats.MissilesFired);
        }

        return sum >= value;
    }

    public Player Player { get; set; }

    public List<Achievement> Achievements = new List<Achievement>();
}
