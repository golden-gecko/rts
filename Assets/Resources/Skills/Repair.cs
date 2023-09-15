using UnityEngine;

public class Repair : Skill
{
    public override object Clone()
    {
        return new Repair(Name, Range, Cooldown.Max, Value);
    }

    public Repair(string name, float range, float cooldown, float value) : base(name, range, cooldown, "Effects/Skills/Healing")
    {
        Value = value;
    }

    public override void Execute(MyGameObject myGameObject)
    {
        foreach (MyGameObject target in Object.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (target.Is(myGameObject, DiplomacyState.Ally) == false)
            {
                continue;
            }

            if (Utils.IsInRange(target.Position, myGameObject.Position, Range) == false)
            {
                continue;
            }

            target.OnRepair(Value);
        }

        Object.Instantiate(Resources.Load(Effect), myGameObject.Position, Quaternion.identity);
    }

    public float Value { get; } = 0.0f;
}
