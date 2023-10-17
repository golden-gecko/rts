using UnityEngine;

public class Missile : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        parent = GetComponent<MyGameObject>();

        if (FireEffectPrefab != null)
        {
            Instantiate(FireEffectPrefab, Position, Quaternion.identity); // TODO: Set in inspector.
        }
    }

    [field: SerializeField]
    public GameObject FireEffectPrefab { get; private set; }

    [field: SerializeField]
    public GameObject HitEffectPrefab { get; private set; }

    [field: SerializeField]
    public Property Damage { get; set; } = new Property();

    [field: SerializeField]
    public Property Range { get; set; } = new Property();

    protected MyGameObject parent { get; private set; }
}
