using UnityEngine;

public class Repair : Skill
{
    public Repair(string name, float cooldown, float range, float value) : base(name, cooldown, range)
    {
        Value = value;
        Effect = "Effects/CFXR3 Hit Misc A";
    }

    public override void Execute(MyGameObject myGameObject)
    {
        foreach (MyGameObject target in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (target.IsAlly(myGameObject) == false)
            {
                continue;
            }

            if (target.IsInRange(myGameObject.Position, Range) == false)
            {
                continue;
            }

            target.OnRepair(Value);
        }

        Object.Instantiate(Resources.Load(Effect), myGameObject.Position, Quaternion.identity); // TODO: Move effect name to configuration.
    }

    public float Value { get; } = 0.0f;
}
