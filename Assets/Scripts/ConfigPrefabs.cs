using System.Collections.Generic;
using UnityEngine;

public class ConfigPrefabs : Singleton<ConfigPrefabs>
{
    [field: SerializeField]
    public GameObject Base { get; private set; }

    [field: SerializeField]
    public GameObject Indicators { get; private set; }

    [field: SerializeField]
    public List<GameObject> Drives { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Guns { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Shields { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Disasters { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Structures { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Units { get; private set; } = new List<GameObject>();
}
