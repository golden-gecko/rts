using UnityEngine;

[DisallowMultipleComponent]
public class Hover : Drive
{
    public override string GetInfo()
    {
        return string.Format("Hover - {0}", base.GetInfo());
    }
}
