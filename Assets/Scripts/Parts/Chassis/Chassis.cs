using UnityEngine;

[DisallowMultipleComponent]
public class Chassis : Part
{
    public override string GetInfo()
    {
        return string.Format("Chassis - {0}", base.GetInfo());
    }
}
