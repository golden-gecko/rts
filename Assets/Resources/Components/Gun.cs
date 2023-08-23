using UnityEngine;

public class Gun : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        Reload.Max = CooldownTime;
    }

    protected override void Update()
    {
        Reload.Update(Time.deltaTime);
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Reload: {1}", base.GetInfo(), Reload.GetInfo());
    }

    public virtual void Fire(MyGameObject myGameObject, Vector3 position)
    {
    }

    [field: SerializeField]
    public float Damage { get; set; } = 10.0f;

    [field: SerializeField]
    public float Range { get; set; } = 10.0f;

    [field: SerializeField]
    public float CooldownTime { get; set; } = 10.0f;

    [field: SerializeField]
    public GameObject MissilePrefab { get; set; }

    [field: SerializeField]
    public string HitEffectPrefab { get; set; }

    public Timer Reload { get; } = new Timer();
}
