using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Foundation : Part
{
    protected override void Awake()
    {
        base.Awake();

        InitializeMapLayers();
    }

    public override string GetInfo()
    {
        return string.Format("Foundation - {0}", base.GetInfo());
    }

    private void InitializeMapLayers()
    {
        foreach (MyGameObjectMapLayer layer in MapLayers)
        {
            if (Parent.MapLayers.Contains(layer) == false)
            {
                Parent.MapLayers.Add(layer);
            }
        }
    }

    [field: SerializeField]
    public List<MyGameObjectMapLayer> MapLayers { get; private set; } = new List<MyGameObjectMapLayer>(); // TODO: Bake terrain type layer. Add terrain types (solid, dirt, grass, swamp etc.).
}
