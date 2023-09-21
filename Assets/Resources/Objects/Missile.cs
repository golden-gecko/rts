using UnityEngine;

public class Missile : MyGameObject // TODO: Add collision with terrain to missiles.
{
    protected override void Start()
    {
        base.Start();

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
    public float Damage { get; set; } = 10.0f;

    [field: SerializeField]
    public float DamageFactor { get; set; } = 1.0f;

    [field: SerializeField]
    public float Range { get; set; } = 10.0f;
}
