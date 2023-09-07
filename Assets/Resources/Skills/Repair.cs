using UnityEngine;

public class Repair : Skill
{
    public Repair(string name, float cooldown, float range, float value) : base(name, cooldown, range)
    {
        Value = value;
        Effect = "Effects/Skills/Healing"; // TODO: Get from property.

    }

    public override void Execute(MyGameObject myGameObject)
    {
        foreach (MyGameObject target in Object.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (target.Is(myGameObject, DiplomacyState.Ally) == false)
            {
                continue;
            }

            if (target.IsInRange(myGameObject.Position, Range) == false)
            {
                continue;
            }

            target.OnRepair(Value);
        }

        Object.Instantiate(Resources.Load(Effect), myGameObject.Position, Quaternion.identity);
    }

    public float Value { get; } = 0.0f;
}
