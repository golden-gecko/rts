using System;
using UnityEngine;

public struct BlueprintComponent
{
    public PartType PartType;
    public Part Part;
    public Vector3 Position;
};

[Serializable]
public struct DamageTypeItem
{
    [field: SerializeField]
    public DamageType Type { get; set; }

    [field: SerializeField, Range(0.0f, 1.0f)]
    public float Ratio { get; set; }
};
