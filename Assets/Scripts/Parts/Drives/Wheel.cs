using UnityEngine;

[DisallowMultipleComponent]
public class Wheel : Drive
{
    public override string GetInfo()
    {
        return string.Format("Wheel - {0}", base.GetInfo());
    }
}
