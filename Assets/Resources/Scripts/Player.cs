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

    public List<MyGameObject> Selected { get; } = new List<MyGameObject>();

    public TechnologyTree TechnologyTree { get; } = new TechnologyTree();
}
