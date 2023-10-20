using System;
using UnityEngine;

[Serializable]
public struct DamageTypeItem
{
    [field: SerializeField]
    public DamageType Type { get; set; }

    [field: SerializeField, Range(0.0f, 1.0f)]
    public float Ratio { get; set; }
};
