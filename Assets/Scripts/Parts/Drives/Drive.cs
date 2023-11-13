using System.Collections.Generic;
using UnityEngine;

public class Drive : Part
{
    [field: SerializeField]
    public List<MyGameObjectMapLayer> MapLayers { get; private set; } = new List<MyGameObjectMapLayer>();
}
