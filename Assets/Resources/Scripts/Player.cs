using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public Sprite Selection;

    public TechnologyTree TechnologyTree { get; } = new TechnologyTree();
}
