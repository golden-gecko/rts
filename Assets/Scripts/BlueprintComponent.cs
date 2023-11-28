using System;
using UnityEngine;

[Serializable]
public class BlueprintComponent
{
    public PartType PartType;

    public string Name;

    [NonSerialized]
    public GameObject Part;

    public Vector3 Position;

    [NonSerialized]
    public GameObject Instance;
};
