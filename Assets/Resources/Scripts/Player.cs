using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        TechnologyTree.Load();
    }

    [SerializeField]
    public Sprite Selection;

    public HashSet<MyGameObject> Selected { get; } = new HashSet<MyGameObject>();

    public TechnologyTree TechnologyTree { get; } = new TechnologyTree();
}
