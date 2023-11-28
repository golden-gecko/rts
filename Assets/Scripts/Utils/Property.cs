using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Property: ICloneable
{
    public object Clone()
    {
        return new Property(Value);
    }

    public Property(float value = 0.0f)
    {
        Value = value;
    }

    [field: SerializeField]
    public float Value { get; private set; }

    public Dictionary<GameObject, float> Factor { get; } = new Dictionary<GameObject, float>();

    public float Total { get => Value * (Factor.Count > 0 ? Factor.Max(x => x.Value) : 1.0f); }
}
