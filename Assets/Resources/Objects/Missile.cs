using UnityEngine;

public class Missile : MyGameObject // TODO: Add collision with terrain to missiles.
{
    protected override void Start()
    {
        base.Start();

        parent = GetComponent<MyGameObject>();

        if (FireEffectPrefab != null)
        {
            Instantiate(FireEffectPrefab, Position, Quaternion.identity);
        }
    }

    [field: SerializeField]
    public GameObject FireEffectPrefab { get; set; }

    [field: SerializeField]
    public GameObject HitEffectPrefab { get; set; }

    [field: SerializeField]
    public Property Damage { get; set; } = new Property();

    [field: SerializeField]
    public Property Range { get; set; } = new Property();

    protected MyGameObject parent { get; private set; }
}
