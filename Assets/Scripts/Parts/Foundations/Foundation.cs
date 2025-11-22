using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Foundation : Part
{
    protected override void Awake()
    {
        base.Awake();

        foreach (MyGameObjectMapLayer layer in MapLayers)
        {
            if (Parent.MapLayers.Contains(layer) == false)
            {
                Parent.MapLayers.Add(layer);
            }
        }
    }

    public override string GetInfo()
    {
        return string.Format("Foundation - {0}", base.GetInfo());
    }

    [field: SerializeField]
    public List<MyGameObjectMapLayer> MapLayers { get; private set; } = new List<MyGameObjectMapLayer>();
}
