using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Missile : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        if (FireEffectPrefab != null)
        {
            Instantiate(FireEffectPrefab, Position, Parent.Rotation);
        }
    }

    [field: SerializeField]
    public GameObject FireEffectPrefab { get; private set; }

    [field: SerializeField]
    public GameObject MoveEffectPrefab { get; private set; }

    [field: SerializeField]
    public GameObject HitEffectPrefab { get; private set; }

    public Property Damage { get; set; } = new Property();

    public Property Range { get; set; } = new Property();

    public List<DamageTypeItem> DamageType { get; set; } = new List<DamageTypeItem>();
}
