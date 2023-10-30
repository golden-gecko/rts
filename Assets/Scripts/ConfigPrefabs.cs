using System.Collections.Generic;
using UnityEngine;

public class ConfigPrefabs : Singleton<ConfigPrefabs>
{
    [field: SerializeField]
    public GameObject Base { get; set; }

    [field: SerializeField]
    public GameObject Indicators { get; set; }

    [field: SerializeField]
    public List<GameObject> Disasters { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Structures { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Units { get; private set; } = new List<GameObject>();
}
