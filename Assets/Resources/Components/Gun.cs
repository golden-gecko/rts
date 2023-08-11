using UnityEngine;

public class Gun : MyComponent
{
    protected override void Update()
    {
        Reload.Update(Time.deltaTime);
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Reload: {1:0.}/{2:0.}", base.GetInfo(), Reload.Current, Reload.Max);
    }

    public virtual void Fire(MyGameObject myGameObject, Vector3 position)
    {
    }

    [field: SerializeField]
    public float Damage { get; set; } = 10.0f;

    [field: SerializeField]
    public float Range { get; set; } = 20.0f;

    [field: SerializeField]
    public string MissilePrefab { get; set; } = "Objects/Missiles/Rocket";

    [field: SerializeField]
    public string HitEffectPrefab { get; set; } = "Effects/CFXR3 Hit Electric C (Air)";

    public Timer Reload { get; } = new Timer(3.0f); // TODO: Create property.
}
