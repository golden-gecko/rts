using UnityEngine;

public class Missile : MyGameObject
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

    public Vector3 Target { get; set; }

    public float Range { get; set; }
}
