using System.Collections.Generic;
using UnityEngine;

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

    [field: SerializeField]
    public List<MyGameObjectMapLayer> MapLayers { get; private set; } = new List<MyGameObjectMapLayer>();
}
