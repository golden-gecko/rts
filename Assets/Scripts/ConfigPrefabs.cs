using System.Collections.Generic;
using UnityEngine;

public class ConfigPrefabs : Singleton<ConfigPrefabs>
{
    [field: SerializeField]
    public GameObject Base { get; private set; }

    [field: SerializeField]
    public GameObject Indicators { get; private set; }

    #region GameObjects
    [field: SerializeField]
    public List<GameObject> Disasters { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Plants { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Rocks { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Structures { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Units { get; private set; } = new List<GameObject>();
    #endregion

    #region Parts
    [field: SerializeField]
    public List<GameObject> Chassis { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Drives { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Guns { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Shields { get; private set; } = new List<GameObject>();
    #endregion
}
