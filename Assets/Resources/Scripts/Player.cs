using UnityEngine;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        TechnologyTree.Load();
    }

    [SerializeField]
    public Sprite Selection;

    public TechnologyTree TechnologyTree { get; } = new TechnologyTree();
}
