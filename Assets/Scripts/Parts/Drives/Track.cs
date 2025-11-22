using UnityEngine;

[DisallowMultipleComponent]
public class Track : Drive
{
    public override string GetInfo()
    {
        return string.Format("Track - {0}", base.GetInfo());
    }
}
