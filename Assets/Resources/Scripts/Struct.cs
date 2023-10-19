using System;
using UnityEngine;

[Serializable]
public struct DamageTypeItem
{
    [field: SerializeField]
    public DamageType Type { get; set; }

    [field: SerializeField]
    public float Ratio { get; set; }
};
