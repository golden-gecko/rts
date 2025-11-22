using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Property: ICloneable
{
    public object Clone()
    {
        return new Property(Base);
    }

    public Property(float base_ = 10.0f) // TODO: Name collision.
    {
        Base = base_;
    }

    [field: SerializeField]
    public float Base { get; set; }

    [field: SerializeField]
    public Dictionary<MyGameObject, float> Factor { get; set; } = new Dictionary<MyGameObject, float>();

    public float Value { get => Base * (Factor.Count > 0 ? Factor.Max(x => x.Value) : 1.0f); }
}
